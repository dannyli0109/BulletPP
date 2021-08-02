using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum ValueType
{
    Double,
    String,
    Bool,
    NativeFunction,
    ImportedFunction,
    Expression,
    Null
}

public class Function
{
    public List<Expression> parameters;
    public Expression body;
    public Env env;
}

public class Value
{

    public Value(double val)
    {
        type = ValueType.Double;
        doubleValue = val;
    }

    public Value(string val)
    {
        type = ValueType.String;
        stringValue = val;
    }

    public Value(bool val)
    {
        type = ValueType.Bool;
        boolValue = val;
    }

    public Value(Function function)
    {
        type = ValueType.NativeFunction;
        nativeFunctionValue = function;
    }

    public Value(Func<List<Value>, Expression> function)
    {
        type = ValueType.ImportedFunction;
        importedFunctionValue = function;
    }

    public Value(Expression exp)
    {
        type = ValueType.Expression;
        expressionValue = exp;
    }

    public Value()
    {
        type = ValueType.Null;
    }
    public ValueType type;

    public string stringValue;
    public double doubleValue;
    public bool boolValue;
    public Function nativeFunctionValue;
    public Function lambdaFunctionValue;
    public Func<List<Value>, Expression> importedFunctionValue;
    public Expression expressionValue;

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }


    public override bool Equals(object obj)
    {
        if (!(obj is Value))
            return false;

        return Equals((Value)obj);
    }

    public bool Equals(Value other)
    {
        return this == other;
    }

    public static bool operator !=(Value a, Value b)
    {
        return !(a == b);
    }
    public static bool operator ==(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return a.doubleValue == b.doubleValue;
                case ValueType.String: return a.stringValue == b.stringValue;
                case ValueType.Bool: return a.boolValue == b.boolValue;
                case ValueType.Null: return true;
            }
        }
        return false;
    }

    public static Value operator +(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue + b.doubleValue);
                case ValueType.String: return new Value(a.stringValue + b.stringValue);
            }
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator *(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue * b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator /(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue / b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator -(Value val)
    {
        switch (val.type)
        {
            case ValueType.Double:
                return new Value(-val.doubleValue);
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator !(Value val)
    {
        switch (val.type)
        {
            case ValueType.Bool:
                {
                    val.boolValue = !val.boolValue;
                    return val;
                }
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator >(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue > b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator <(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue < b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }

    public static Value operator >=(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue >= b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }


    public static Value operator <=(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue <= b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }


    public static Value operator -(Value a, Value b)
    {
        if (a.type == b.type)
        {
            switch (a.type)
            {
                case ValueType.Double: return new Value(a.doubleValue - b.doubleValue);
            }
        }
        throw new Exception("Operation not allow");
    }

    public Expression GetExpression()
    {
        if (type == ValueType.Double)
        {
            return new AST()
            {
                type = "NumericLiteral",
                value = this
            };
        };
        if (type == ValueType.String)
        {
            return new AST()
            {
                type = "StringLiteral",
                value = this
            };
        }

        if (type == ValueType.NativeFunction)
        {
            return new AST()
            {
                type = "Null",
                value = this
            };
        }


        if (type == ValueType.ImportedFunction)
        {
            return new AST()
            {
                type = "Null",
                value = this
            };
        }

        if (type == ValueType.Expression)
        {
            return expressionValue;
        }

        if (type == ValueType.Null)
        {
            return new AST()
            {
                type = "Null",
                value = this
            };
        }
        if (type == ValueType.Bool)
        {
            return new AST()
            {
                type = "BooleanLiteral",
                value = this
            };
        }
        //if (value.type == ValueType.Bool) return new Exp(value.boolValue);
        return null;
    }
}
public class Env
{
    Dictionary<string, Value> record;
    Env parent;
    public Env(Dictionary<string, Value> record = null, Env parent = null)
    {
        if (record == null) this.record = new Dictionary<string, Value>();
        else this.record = record;

        if (parent == null) this.parent = null;
        else this.parent = parent;
    }

    public Expression Define(string name, Value value)
    {
        record[name] = value;
        return value.GetExpression();
    }

    public Expression LookUp(string name)
    {
        Value value = Resolve(name).record[name];
        return value.GetExpression();
    }

    public Expression Set(string name, Value value)
    {
        Resolve(name).record[name] = value;
        return value.GetExpression();
    }

    public Env Resolve(string name)
    {
        if (record.ContainsKey(name))
        {
            return this;
        }

        if (parent == null)
        {
            return null;
        }

        return parent.Resolve(name);
    }

}
