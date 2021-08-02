using System;
using System.Collections.Generic;

public class Expression
{
    public string type;
    public Value value;
    public List<Expression> body;
    public Expression expression;
    //public string binaryOperator;
    public string operator_;
    public string name;
    public Expression id;
    public Expression init;
    public List<Expression> declarations;
    public Expression left;
    public Expression right;

    public Expression test;
    public Expression consequent;
    public Expression alternate;

    public Expression argument;

    public Expression update;
    public List<Expression> parameters;

    public bool computed;
    public Expression obj;
    public Expression property;

    public Expression callee;
    public List<Expression> arguments;
    public bool executed = true;

    //public Value value
    //{
    //    get
    //    {
    //        if (type == "StringLiteral") return new Value(stringValue);
    //        if (type == "NumericLiteral") return new Value(douebleValue);
    //        return new Value();
    //    }
    //}
}
public class AST : Expression
{

}

interface Factory
{
    Expression Program(List<Expression> body);
    Expression EmptyStatement();
    Expression BlockStatement(List<Expression> body);
    Expression ExpressionStatement(Expression expression);
    Expression StringLiteral(string str);
    Expression NumericLiteral(double number);
}

class DefaultFactory : Factory
{
    public Expression BlockStatement(List<Expression> body)
    {
        return new AST()
        {
            type = "BlockStatement",
            body = body
        };
    }

    public Expression EmptyStatement()
    {
        return new AST()
        {
            type = "EmptyStatement"
        };
    }

    public Expression ExpressionStatement(Expression expression)
    {
        return new AST()
        {
            type = "ExpressionStatement",
            expression = expression
        };
    }

    public Expression NumericLiteral(double number)
    {
        return new AST()
        {
            type = "NumericLiteral",
            value = new Value(number)
        };
    }

    public Expression Program(List<Expression> body)
    {
        return new AST()
        {
            type = "Program",
            body = body
        };
    }

    public Expression StringLiteral(string str)
    {
        return new AST()
        {
            type = "StringLiteral",
            value = new Value(str)
        };
    }
}

public class Parser
{
    string str;
    Tokenizer tokenizer;
    Token lookahead;
    Factory factory;

    public Parser()
    {
        factory = new DefaultFactory();
        str = "";
        tokenizer = new Tokenizer();
    }
    public Expression Parse(string str)
    {
        this.str = str;
        tokenizer.Init(str);
        lookahead = tokenizer.GetNextToken();
        if (lookahead == null) return new AST()
        {
            type = "NullLiteral",
            value = new Value()
        };
        return Program();
    }

    /**
     *  Main Entry Point
     *
     *  Program
     *      : Literal
     *      ;
     */

    public Expression Program()
    {
        return factory.Program(StatementList());
    }

    /**
     * StatementList
     *  : Statement
     *  | StatementList Statement
     *  ;
     */

    public List<Expression> StatementList(string stopLookahead = null)
    {
        List<Expression> statementList = new List<Expression>() { Statement() };
        while (lookahead != null && lookahead.type != stopLookahead)
        {
            statementList.Add(Statement());
        }

        return statementList;
    }

    /**
     * Statement
     * : ExpressionStatement
     * | BlockStatement
     * | EmptyStatement
     * | VariableStatement
     * | IfStatement
     * | IterationStatement
     * | FunctionDeclaration
     * ;
     */


    public Expression Statement()
    {
        switch (lookahead.type)
        {
            case ";": return EmptyStatement();
            case "if": return IfStatement();
            case "{": return BlockStatement();
            case "let": return VariableStatement();
            case "while":
            case "do":
            case "for":
                return IterationStatement();
            case "def": return FunctionDeclaration();
            case "lambda": return LambdaFunctionExpression();
            case "return": return ReturnStatement();
            default: return ExpressionStatement();
        }
    }

    /**
     * ReturnStatement
     *  : "return" OptExpression ";"
     *  ;
     */
    public Expression ReturnStatement()
    {
        Eat("return");
        Expression argument = lookahead.type != ";" ? Expression() : null;
        return new AST()
        {
            type = "ReturnStatement",
            argument = argument
        };
    }

    /**
     * ArrowFunctionExpression
     *  : lambda "(" OptFormalParameterList ")" BlockStatement
     *;
     */
    public Expression LambdaFunctionExpression()
    {
        Eat("lambda");
        Eat("(");
        List<Expression> parameters = FormalParameterList();
        Eat(")");

        Expression body = BlockStatement();
        return new AST()
        {
            type = "LambdaFunctionExpression",
            parameters = parameters,
            body = new List<Expression>() { body }
        };
    }

    /**
     * FunctionDeclaration
     *  : "def" Identifier "(" OptFormalParameterList ")" BlockStatement
     *  ;
     */

    public Expression FunctionDeclaration()
    {
        Eat("def");
        Expression id = Identifier();
        Eat("(");
        List<Expression> parameters = FormalParameterList();
        Eat(")");
        Expression body = BlockStatement();
        return new AST()
        {
            type = "FunctionDeclaration",
            id = id,
            parameters = parameters,
            body = new List<Expression>() { body }
        };
    }

    /**
     * FormalParameterList
     *  : Identifier
     *  | FormalParameterList "," Identifier
     *  ;
     */

    public List<Expression> FormalParameterList()
    {
        List<Expression> parameters = new List<Expression>();
        if (lookahead.type == ")") return parameters;
        do
        {
            if (lookahead.type == ",") Eat(",");
            parameters.Add(Identifier());
        } while (lookahead.type == ",");
        return parameters;
    }


    /**
     * IterationStatement
     *  : WhileStatement
     *  | DoWhileStatement
     *  | ForStatement
     *  ;
     */

    public Expression IterationStatement()
    {
        switch (lookahead.type)
        {
            case "while": return WhileStatement();
            case "do": return DoWhileStatement();
            case "for": return ForStatement();
            default:
                break;
        }

        throw new Exception("Unknown Iteration Statement Type: " + lookahead.type);
    }

    /**
     * WhileStatement
     * : 'while' '(' Expression ')' Statement
     * ;
     */

    public Expression WhileStatement()
    {
        Eat("while");
        Eat("(");
        Expression test = Expression();
        Eat(")");
        Expression body = Statement();

        return new AST()
        {
            type = "WhileStatement",
            test = test,
            body = new List<Expression>() { body }
        };
    }

    /**
     * DoWhileStatement
     *  : 'do' Statement 'while' '(' Expression ')' ';'
     *  ;
     */
    public Expression DoWhileStatement()
    {
        Eat("do");
        Expression body = Statement();
        Eat("while");
        Eat("(");
        Expression test = Expression();
        Eat(")");
        Eat(";");
        return new AST()
        {
            type = "DoWhileStatement",
            test = test,
            body = new List<Expression>() { body }
        };
    }

    /**
     * ForStatement
     *  : "for" "(" OptForStatementInit ";" OptExpression ";" OptExpression ")" Statement
     *  ;
     */

    public Expression ForStatement()
    {
        Eat("for");
        Eat("(");
        Expression init = lookahead.type != ";" ? ForStatementInit() : null;
        Eat(";");
        Expression test = lookahead.type != ";" ? Expression() : null;
        Eat(";");
        Expression update = lookahead.type != ")" ? Expression() : null;
        Eat(")");
        Expression body = Statement();

        return new AST()
        {
            type = "ForStatement",
            init = init,
            test = test,
            update = update,
            body = new List<Expression>() { body }
        };
    }

    /**
     * ForStatementInit
     *  : VariableStaementInit
     *  | Expression
     *;
     */

    public Expression ForStatementInit()
    {
        if (lookahead.type == "let")
        {
            return VariableStatementInit();
        }
        return Expression();
    }


    /**
     * IfStatement
     *  : 'if' '(' Expression ')' Statement
     *  | 'if' '(' Expression ')' Statement 'else' Statement
     *  ;
     */

    public Expression IfStatement()
    {
        Eat("if");
        Eat("(");
        Expression test = Expression();
        Eat(")");
        Expression consequent = Statement();

        Expression alternate = null;
        if (lookahead != null && lookahead.type == "else")
        {
            Eat("else");
            alternate = Statement();
        }
        return new AST()
        {
            type = "IfStatement",
            test = test,
            consequent = consequent,
            alternate = alternate
        };
    }

    /**
     * VariableStatementInit
     *  : "let" VariableDeclarationList
     *  ;
     */

    public Expression VariableStatementInit()
    {
        Eat("let");
        List<Expression> declarations = VariableDeclarationList();
        return new AST()
        {
            type = "VariableStatement",
            declarations = declarations
        };
    }

    /**
     * VariableStatement
     * : 'let' VariableDeclarationList ';'
     * ;
     */

    public Expression VariableStatement()
    {
        Expression variableStatement = VariableStatementInit();
        Eat(";");
        return variableStatement;
    }

    /**
     * VariableDeclarationList
     *  : VariableDeclaration
     *  | VariableDeclarationList ',' VariableDeclaration
     *  ;
     */

    public List<Expression> VariableDeclarationList()
    {
        List<Expression> declarations = new List<Expression>();
        do
        {
            if (lookahead.type == ",") Eat(",");
            declarations.Add(VariableDeclaration());
        } while (lookahead.type == ",");
        return declarations;
    }

    /**
     * VariableDeclaration
     *  : Identifier OptVariableInitializer
     *  ;
     */
    public Expression VariableDeclaration()
    {
        Expression id = Identifier();

        Expression init = lookahead.type != ";" && lookahead.type != ","
            ? VariableInitializer()
            : null;

        return new AST()
        {
            type = "VariableDeclaration",
            id = id,
            init = init
        };
    }

    /**
     * VariableInitializer
     *  : SIMPLE_ASSIGN AssignmentExpression
     *  ;
     */

    public Expression VariableInitializer()
    {
        Eat("SIMPLE_ASSIGN");
        return AssignmentExpression();
    }


    /**
     * EmptyStatement
     *  : ;
     *  ;
     */

    public Expression EmptyStatement()
    {
        Eat(";");
        return factory.EmptyStatement();
    }

    /**
     * BlockStatement
     *  : '{' OptStatementList '}'
     *  ;
     */

    public Expression BlockStatement()
    {
        Eat("{");
        List<Expression> body = lookahead.type != "}" ? StatementList("}") : new List<Expression>();
        Eat("}");
        return factory.BlockStatement(body);
    }


    /**
     * ExpressionStatement
     *  : Expression ';'
     *  ;
     */
    public Expression ExpressionStatement()
    {
        Expression expression = Expression();
        Eat(";");
        return factory.ExpressionStatement(expression);
    }

    /**
     * Expression
     *  : Literal
     *  ;
     */

    public Expression Expression()
    {
        return AssignmentExpression();
    }

    /**
     * AssignmentExpression
     *  : LogicalORExpression
     *  | LeftHandSideExpression AssignmentOperator AssignmentExpressio
     *  ;
     */

    public Expression AssignmentExpression()
    {
        Expression left = LogicalORExpression();

        if (!_IsAssignmentOperator(lookahead.type))
        {
            return left;
        }

        string operator_ = AssignmentOperator().value;

        switch (operator_)
        {
            case "++":
                return new AST()
                {
                    type = "AssignmentExpression",
                    operator_ = "+=",
                    left = _checkValidAssignmentTarget(left),
                    right = factory.NumericLiteral(1)
                };
            case "--":
                return new AST()
                {
                    type = "AssignmentExpression",
                    operator_ = "-=",
                    left = _checkValidAssignmentTarget(left),
                    right = factory.NumericLiteral(1)
                };
            default:
                break;
        }
        return new AST()
        {
            type = "AssignmentExpression",
            operator_ = operator_,
            left = _checkValidAssignmentTarget(left),
            right = AssignmentExpression()
        };
    }

    /**
     * LeftHandSideExpression
     *  : CallMemberExpression
     *  ;
     */

    public Expression LeftHandSideExpression()
    {
        return CallMemberExpression();
    }

    /**
     * CallMemberExpression
     *  : MemberExpression
     *  | CallExpression
     *  ;
     */

    public Expression CallMemberExpression()
    {
        Expression member = MemberExpression();

        if (lookahead.type == "(")
        {
            return _CallExpression(member);
        }
        return member;
    }

    /**
     * Generic call expression helper.
     * 
     * CallExpression
     *  : Callee Arguments
     *  ;
     *  
     * Callee
     *  : MemberExpression
     *  | CallExpresion
     *  ;
     */
    public Expression _CallExpression(Expression callee)
    {
        Expression callExpression = new AST()
        {
            type = "CallExpression",
            callee = callee,
            arguments = Arguments()
        };

        if (lookahead.type == "(")
        {
            callExpression = _CallExpression(callExpression);
        }
        return callExpression;
    }

    /**
     * Arguments
     *  : "(" OptArgumentList ")"
     *  ;
     */

    public List<Expression> Arguments()
    {
        Eat("(");
        List<Expression> argumentList = ArgumentList();
        Eat(")");
        return argumentList;
    }

    /**
     * ArgumentList
     *  : AssignmentExpression
     *  | ArgumentList "," AssignmentExpression
     *  ;
     */

    public List<Expression> ArgumentList()
    {
        List<Expression> argumentList = new List<Expression>();
        if (lookahead.type == ")") return argumentList;
        do
        {
            if (lookahead.type == ",") Eat(",");
            argumentList.Add(AssignmentExpression());
        } while (lookahead.type == ",");
        return argumentList;
    }

    /**
     * MemberExpression
     *  : PrimaryExpression
     *  | MemberExpression "." Identifier
     *  | MemberExpression "[" Expression "]"
     *  ;
     */

    public Expression MemberExpression()
    {
        Expression obj = PrimaryExpression();
        while (lookahead.type == "." || lookahead.type == "[")
        {
            if (lookahead.type == ".")
            {
                Eat(".");
                Expression property = Identifier();
                obj = new AST()
                {
                    type = "MemberExpression",
                    computed = false,
                    obj = obj,
                    property = property
                };
            }

            if (lookahead.type == "[")
            {
                Eat("[");
                Expression property = Expression();
                Eat("]");
                obj = new AST()
                {
                    type = "MemberExpression",
                    computed = true,
                    obj = obj,
                    property = property
                };
            }
        }
        return obj;
    }

    /**
     * Identifier
     *  : IDENTIFIER
     *  ;
     */
    public Expression Identifier()
    {
        string name = Eat("IDENTIFIER").value;
        return new AST()
        {
            type = "Identifier",
            name = name
        };
    }

    Expression _checkValidAssignmentTarget(Expression node)
    {
        if (node.type == "Identifier" || node.type == "MemberExpression")
        {
            return node;
        }

        throw new Exception("Invalid left-hand side in assignment expression");
    }

    /**
     * AssignmentOperator
     *  : SIMPLE_ASSIGN
     *  | COMPLEX_ASSIGN
     *  ;
     */

    public Token AssignmentOperator()
    {
        if (lookahead.type == "SIMPLE_ASSIGN")
        {
            return Eat("SIMPLE_ASSIGN");
        }
        else if (lookahead.type == "INCREMENT_ASSIGN")
        {
            return Eat("INCREMENT_ASSIGN");
        }
        else if (lookahead.type == "DECREMENT_ASSIGN")
        {
            return Eat("DECREMENT_ASSIGN");
        }
        return Eat("COMPLEX_ASSIGN");
    }

    /**
     * Logical OR expression
     * 
     *  x || y
     *  LogicalORExpression
     *      : LogicalANDExpression LOGICAL_OR LogicalORExpression
     *      | EqualityExpression
     *      ;
     */

    public Expression LogicalORExpression()
    {
        return _LogicalExpression(LogicalANDExpression, "LOGICAL_OR");
    }

    /**
     * Logical AND expression
     * 
     *  x && y
     *  LogicalANDExpression
     *      : EqualityExpression LOGICAL_AND LogicalANDExpression
     *      | EqualityExpression
     */

    public Expression LogicalANDExpression()
    {
        return _LogicalExpression(EqualityExpression, "LOGICAL_AND");
    }

    public Expression _LogicalExpression(Func<Expression> builder, string operatorToken)
    {
        Expression left = builder();
        while (lookahead.type == operatorToken)
        {
            string binaryOperator = Eat(operatorToken).value;
            Expression right = builder();

            left = new AST()
            {
                type = "LogicalExpression",
                operator_ = binaryOperator,
                left = left,
                right = right
            };
        }
        return left;
    }

    /**
     * EQUALITY_OPERATOR: ==, !==
     * 
     *  x == y
     *  x != y
     *  
     *  EqualityExpression
     *      : RelationalExpression EQUALITY_OPERATOR EqualityExpression
     *      | RelationalExpression
     *      ;
     */

    public Expression EqualityExpression()
    {
        return _BinaryExpression(RelationalExpression, "EQUALITY_OPERATOR");
    }

    /**
     * RelationalOperator: >, >=, <, <=
     * 
     * x > y
     * x >= y
     * x < y
     * x <= y
     * 
     * RelationalExpression
     *  : AdditiveExpression
     *  | AdditiveExpression RELATIONAL_OPERATOR RelationalExpression
     *  ;
     */
    public Expression RelationalExpression()
    {
        return _BinaryExpression(AdditiveExpression, "RELATIONAL_OPERATOR");
    }



    bool _IsAssignmentOperator(string type)
    {
        return type == "SIMPLE_ASSIGN" || type == "COMPLEX_ASSIGN" ||
            type == "INCREMENT_ASSIGN" || type == "DECREMENT_ASSIGN";
    }



    /**
     * AdditiveExpression
     *  : Literal
     *  | AdditiveExpression ADDITIVE_OPERATOR Literal
     */

    public Expression AdditiveExpression()
    {
        return _BinaryExpression(MultiplicativeExpression, "ADDITIVE_OPERATOR");
    }

    /**
     * MultiplicativeExpression
     *  : PrimaryExpression
     *  | MultiplicativeExpression MULTIPLICATIVE_OPERATOR PrimaryExpression
     *  ;
     */

    public Expression MultiplicativeExpression()
    {
        return _BinaryExpression(UnaryExpression, "MULTIPLICATIVE_OPERATOR");
    }

    Expression _BinaryExpression(Func<Expression> builder, string operatorToken)
    {
        Expression left = builder();
        while (lookahead.type == operatorToken)
        {
            string binaryOperator = Eat(operatorToken).value;
            Expression right = builder();

            left = new AST()
            {
                type = "BinaryExpression",
                operator_ = binaryOperator,
                left = left,
                right = right
            };
        }
        return left;
    }

    /**
     * UnaryExpression
     *  : LeftHandSideExpression
     *  | ADDITIVE_OPERATOR UnaryExpression
     *  | LOGICAL_NOT UnaryExpression
     *  ;
     */

    public Expression UnaryExpression()
    {
        string operator_ = null;
        switch (lookahead.type)
        {
            case "ADDITIVE_OPERATOR":
                {
                    operator_ = Eat("ADDITIVE_OPERATOR").value;
                    break;
                }
            case "LOGICAL_NOT":
                {
                    operator_ = Eat("LOGICAL_NOT").value;
                    break;
                }
            default:
                break;
        }

        if (operator_ != null)
        {
            return new AST()
            {
                type = "UnaryExpression",
                operator_ = operator_,
                argument = UnaryExpression()
            };
        }
        return LeftHandSideExpression();
    }


    /**
     * PrimaryExpression
     *  : Literal
     *  | ParenthesizedExpression
     *  | Identifier
     *  ;
     */

    public Expression PrimaryExpression()
    {
        if (_IsLiteral(lookahead.type))
        {
            return Literal();
        }

        switch (lookahead.type)
        {
            case "(": return ParenthesizedExpression();
            case "IDENTIFIER": return Identifier();
            case "lambda": return LambdaFunctionExpression();
            default: return LeftHandSideExpression();
        }
    }

    bool _IsLiteral(string type)
    {
        return type == "NUMBER" ||
            type == "STRING" ||
            type == "null" ||
            type == "true" ||
            type == "false";
    }

    /**
     * ParenthesizedExpression
     * : '(' Expression ')'
     * ;
     */

    public Expression ParenthesizedExpression()
    {
        Eat("(");
        Expression expression = Expression();
        Eat(")");
        return expression;
    }


    /**
     * Literal
     *  : NumericLiteral
     *  | StringLiteral
     *  | BooleanLiteral
     *  | NullLiteral
     *  ;
     */

    public Expression Literal()
    {
        switch (lookahead.type)
        {
            case "NUMBER": return NumericLiteral();
            case "STRING": return StringLiteral();
            case "true": return BooleanLiteral(true);
            case "false": return BooleanLiteral(false);
            case "null": return NullLiteral();
        }
        throw new Exception("Literal: unexpected literal production");
    }

    public Expression BooleanLiteral(bool value)
    {
        Eat(value ? "true" : "false");
        return new AST()
        {
            type = "BooleanLiteral",
            value = new Value(value)
        };
    }

    public Expression NullLiteral()
    {
        Eat("null");
        return new AST()
        {
            type = "NullLiteral",
            value = new Value()
        };
    }
    public Expression StringLiteral()
    {
        Token token = Eat("STRING");
        return factory.StringLiteral(token.value.Substring(1, token.value.Length - 2));
    }

    public Expression NumericLiteral()
    {
        Token token = Eat("NUMBER");
        return factory.NumericLiteral(double.Parse(token.value));
    }

    public Token Eat(string tokenType)
    {
        Token token = lookahead;
        if (token == null)
        {
            throw new Exception("Unexpected end of input, expected: " + tokenType);
        }

        if (token.type != tokenType)
        {
            throw new Exception("Unexpected token: " + token.value + ", expected: " + tokenType);
        }

        lookahead = tokenizer.GetNextToken();
        return token;
    }
}
