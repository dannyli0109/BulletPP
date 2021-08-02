using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Eva
{
    Env global;
    public Eva(Env global = null)
    {
        if (global == null) this.global = new Env();
        else this.global = global;
    }

    public Expression eval(Expression exp, Env env = null)
    {
        if (env == null) env = global;
        if (exp == null) return null;

        switch (exp.type)
        {
            case "Program":
            case "BlockStatement":
                return BlockStatement(exp, env);
            case "BinaryExpression":
                return BinaryExpression(exp, env);
            case "UnaryExpression":
                return UnaryExpression(exp, env);
            case "LogicalExpression":
                return LogicalExpression(exp, env);
            case "IfStatement":
                return IfStatement(exp, env);
            case "ExpressionStatement":
                return ExpressionStatement(exp, env);
            case "VariableStatement":
                return VariableStatement(exp, env);
            case "VariableDeclaration":
                return VariableDeclaration(exp, env);
            case "AssignmentExpression":
                return AssignmentExpression(exp, env);
            case "WhileStatement":
                return WhileStatement(exp, env);
            case "DoWhileStatement":
                return DoWhileStatement(exp, env);
            case "ForStatement":
                return ForStatement(exp, env);
            case "FunctionDeclaration":
                return FunctionDeclaration(exp, env);
            case "CallExpression":
                return CallExpression(exp, env);
            case "LambdaFunctionExpression":
                return LambdaFunctionExpression(exp, env);
            case "ReturnStatement":
                return ReturnStatement(exp, env);
            case "Identifier":
                return Identifier(exp, env);
            case "NumericLiteral":
            case "StringLiteral":
            case "BooleanLiteral":
            case "NullLiteral":
                return Literal(exp);
            default:
                break;
        }
        return null;
    }

    public Expression BlockStatement(Expression exp, Env env)
    {
        Env blockEnv = new Env(null, env);
        return evalBlock(exp, blockEnv);
    }

    public Expression BinaryExpression(Expression exp, Env env)
    {
        switch (exp.operator_)
        {
            case "+":
                return new AST()
                {
                    type = "NumericLiteral",
                    value = eval(exp.left, env).value + eval(exp.right, env).value
                };
            case "-":
                return new AST()
                {
                    type = "NumericLiteral",
                    value = eval(exp.left, env).value - eval(exp.right, env).value
                };
            case "*":
                return new AST()
                {
                    type = "NumericLiteral",
                    value = eval(exp.left, env).value * eval(exp.right, env).value
                };
            case "/":
                return new AST()
                {
                    type = "NumericLiteral",
                    value = eval(exp.left, env).value / eval(exp.right, env).value
                };
            case ">":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = eval(exp.left, env).value > eval(exp.right, env).value
                };
            case ">=":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = eval(exp.left, env).value >= eval(exp.right, env).value
                };
            case "<":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = eval(exp.left, env).value < eval(exp.right, env).value
                };
            case "<=":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = eval(exp.left, env).value <= eval(exp.right, env).value
                };
            case "==":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = new Value(eval(exp.left, env).value == eval(exp.right, env).value ? true : false)
                };
            case "!=":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = new Value(eval(exp.left, env).value != eval(exp.right, env).value ? true : false)
                };
            default:
                break;
        }

        throw new Exception("Unknown binary expression");
    }

    public Expression UnaryExpression(Expression exp, Env env)
    {
        Expression argument = exp.argument;
        string operator_ = exp.operator_;
        switch (operator_)
        {
            case "+":
                return new AST()
                {
                    type = "NumericLiteral",
                    value = eval(argument, env).value
                };
            case "-":
                return new AST()
                {
                    type = "NumericLiteral",
                    value = -eval(argument, env).value
                };
            case "!":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = !eval(argument, env).value
                };
            default:
                break;
        }
        throw new Exception("Unknown unary expression");
    }

    public Expression IfStatement(Expression exp, Env env)
    {
        Expression condition = exp.test;
        Expression consequent = exp.consequent;
        Expression alternative = exp.alternate;
        if (eval(condition, env).value == new Value(true)) return eval(consequent, env);
        else if (alternative == null)
        {
            return new Expression()
            {
                executed = false
            };
        }
        else return eval(alternative, env);
    }

    public Expression ExpressionStatement(Expression exp, Env env)
    {
        return eval(exp.expression, env);
    }

    public Expression VariableStatement(Expression exp, Env env)
    {
        return declearVariables(exp, env);
    }

    public Expression VariableDeclaration(Expression exp, Env env)
    {
        string name = exp.id.name;
        if (isVariableName(name))
        {
            Value value;
            if (exp.init != null)
            {
                value = eval(exp.init, env).value;
            }
            else
            {
                value = new Value();
            }
            return env.Define(name, value);
        }
        throw new Exception("Invalid identifier name");
    }

    public Expression AssignmentExpression(Expression exp, Env env)
    {
        string name = exp.left.name;
        string operator_ = exp.operator_;
        if (isVariableName(name))
        {
            Value value = eval(exp.right, env).value;
            Value lookup = env.LookUp(name).value;
            switch (operator_)
            {
                case "=":
                    return env.Set(name, value);
                case "-=":
                    return env.Set(name, lookup - value);
                case "+=":
                    return env.Set(name, lookup + value);
                case "*=":
                    return env.Set(name, lookup * value);
                case "/=":
                    return env.Set(name, lookup / value);
                default:
                    break;
            }
        }
        throw new Exception("Invalid identifier name");
    }

    public Expression WhileStatement(Expression exp, Env env)
    {
        Expression test = exp.test;
        Expression consequent = exp.body[0];
        Expression result = null;

        while (eval(test, env).value.boolValue)
        {
            result = eval(consequent, env);
        }
        return result;
    }

    public Expression DoWhileStatement(Expression exp, Env env)
    {
        Expression test = exp.test;
        Expression consequent = exp.body[0];
        Expression result;
        do
        {
            result = eval(consequent, env);
        } while (eval(test, env).value.boolValue);
        return result;
    }

    public Expression ForStatement(Expression exp, Env env)
    {
        Expression init = exp.init;
        Expression test = exp.test;
        Expression update = exp.update;
        Expression consequent = exp.body[0];
        Expression result = null;

        Env blockEnv = new Env(null, env);
        eval(init, blockEnv);
        while (eval(test, blockEnv).value.boolValue)
        {
            result = eval(consequent, blockEnv);
            eval(update, blockEnv);
        }
        return result;
    }

    public Expression FunctionDeclaration(Expression exp, Env env)
    {
        Expression id = exp.id;
        List<Expression> parameters = exp.parameters;
        Expression body = exp.body[0];

        Function func = new Function()
        {
            parameters = parameters,
            body = body,
            env = env
        };

        return env.Define(id.name, new Value(func));
    }

    public Expression LambdaFunctionExpression(Expression exp, Env env)
    {
        List<Expression> parameters = exp.parameters;
        Expression body = exp.body[0];

        Function func = new Function()
        {
            parameters = parameters,
            body = body,
            env = env
        };

        return (new Value(func).GetExpression());
    }

    public Expression CallExpression(Expression exp, Env env)
    {
        Expression callee = exp.callee;
        Value functionVal = env.LookUp(callee.name).value;
        List<Expression> arguments = exp.arguments;

        switch (functionVal.type)
        {
            case ValueType.NativeFunction:
                {
                    Function function = functionVal.nativeFunctionValue;
                    List<Expression> parameters = function.parameters;
                    Dictionary<string, Value> activationRecord = new Dictionary<string, Value>();
                    for (int i = 0; i < function.parameters.Count; i++)
                    {
                        activationRecord.Add(parameters[i].name, eval(arguments[i], env).value);
                    }

                    Env activationEnv = new Env(activationRecord, function.env);

                    return evalBody(function.body, activationEnv);
                }
            case ValueType.ImportedFunction:
                {
                    List<Value> obj = new List<Value>();
                    for (int i = 0; i < arguments.Count; i++)
                    {
                        obj.Add(eval(arguments[i], env).value);
                    }
                    Func<List<Value>, Expression> f = env.LookUp(callee.name).value.importedFunctionValue;
                    return f(obj);
                }
            default:
                break;
        }
        return null;
    }

    public Expression evalBody(Expression body, Env env)
    {
        if (body.type == "BlockStatement")
        {
            return evalBlock(body, env);
        }
        return eval(body, env);
    }

    public Expression ReturnStatement(Expression exp, Env env)
    {
        Expression arguement = exp.argument;
        return eval(arguement, env);
    }

    public Expression Identifier(Expression exp, Env env)
    {
        if (isVariableName(exp.name))
        {
            return env.LookUp(exp.name);
        }
        throw new Exception("Invalid identifier name");
    }

    public Expression LogicalExpression(Expression exp, Env env)
    {
        string operator_ = exp.operator_;
        switch (exp.operator_)
        {
            case "&&":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = new Value(eval(exp.left, env).value.boolValue && eval(exp.right, env).value.boolValue)
                };
            case "||":
                return new AST()
                {
                    type = "BooleanLiteral",
                    value = new Value(eval(exp.left, env).value.boolValue && eval(exp.right, env).value.boolValue)
                };
        }
        throw new Exception("Invalid Logical Expression: " + exp);
    }

    public Expression Literal(Expression exp)
    {
        return exp;
    }

    public bool isVariableName(string name)
    {
        Regex r = new Regex(@"^[a-zA-Z][a-zA-Z0-9_]*$");
        return r.IsMatch(name);
    }

    public Expression evalBlock(Expression block, Env env)
    {
        Expression result = null;
        for (int i = 0; i < block.body.Count; i++)
        {
            Expression exp = block.body[i];
            Expression temp = eval(exp, env);
            if (temp != null && temp.executed)
            {
                result = temp;
            }
        }
        return result;
    }

    public Expression declearVariables(Expression statement, Env env)
    {
        Expression result = null;
        for (int i = 0; i < statement.declarations.Count; i++)
        {
            Expression exp = statement.declarations[i];
            result = eval(exp, env);
        }
        return result;
    }
}
