using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Models.dbHandler
{
    /// <summary>
    /// 解析Expression
    /// </summary>
    public class WhereExpression
    {
        private int Index = 0;
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Parametets = new Dictionary<string, object>();

        /// <summary>
        /// 成员
        /// </summary>
        /// <param name="expression"></param>
        public KeyValuePair<string, object> ToSql(Expression expression)
        {
            //解析Expression
            string text = ExpressionVisitor(expression);
            if (text != string.Empty)
            {
                text = string.Format(" WHERE {0} ", text);
            }
            return new KeyValuePair<string, object>(text, Parametets.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)).ToArray());
        }

        /// <summary>
        /// 解析Expression
        /// </summary>
        /// <param name="express">表达式</param>
        /// <returns>Sql</returns>
        internal string ExpressionVisitor(Expression exp)
        {
            if (exp == null) return string.Empty;

            if (exp is UnaryExpression)
            {
                //一元运算符有1个操作数。在SQL中体现为：->只留一个反(非) ！
                return UnaryClauses(exp as UnaryExpression);
            }
            else if (exp is BinaryExpression)
            {
                //二元运算符有2个操作数。在SQL中体现为：比较运算符= <> > >= <= < ，以及从属逻辑运算符
                return BinaryClauses(exp as BinaryExpression);
            }
            else if (exp is MethodCallExpression)
            {
                //方法调用 即 is not null ,is null,Contains
                return MethodCallClauses(exp as MethodCallExpression);
            }
            throw new Exception("未实现的Where<T>(Lambda:" + exp.NodeType + ")表达式！请使用Where条件来操作！");
        }
        /// <summary>
        /// 一元
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private string UnaryClauses(UnaryExpression exp)
        {
            //实在想不出一元怎么对应SQL
            switch (exp.NodeType)
            {
                case ExpressionType.Not:
                    return MethodCallClauses(exp.Operand as MethodCallExpression, false);
                default:
                    throw new Exception("未实现的Where<T>(Lambda:" + exp.NodeType + ")表达式！请使用Where条件来操作！");
            }

        }
        /// <summary>
        /// 二元
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private string BinaryClauses(BinaryExpression exp)
        {
            switch (exp.NodeType)
            {
                //逻辑运算符
                //注意：此处不分别短路 && || 统一按& | 算
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return string.Format("{0} {1} {2}", ExpressionVisitor(exp.Left), "AND", ExpressionVisitor(exp.Right));
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return string.Format("{0} {1} {2}", ExpressionVisitor(exp.Left), "OR", ExpressionVisitor(exp.Right));
                //二元操作符
                case ExpressionType.Equal:
                    return ClauseText(exp, "=");
                case ExpressionType.GreaterThan:
                    return ClauseText(exp, ">");
                case ExpressionType.GreaterThanOrEqual:
                    return ClauseText(exp, ">=");
                case ExpressionType.LessThan:
                    return ClauseText(exp, "<");
                case ExpressionType.LessThanOrEqual:
                    return ClauseText(exp, "<=");
                case ExpressionType.NotEqual:
                    return ClauseText(exp, "<>");
                default:
                    throw new Exception("未实现的Where<T>(Lambda:" + exp.NodeType + ")表达式！请使用Where条件来操作！");
            }
        }
        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private string MethodCallClauses(MethodCallExpression ex, bool tag = true)
        {
            var member = ex.Object as MemberExpression;
            var name = member.Member.Name;
            var value
                = ex.Arguments[0].NodeType == ExpressionType.Constant
                     ? ((ConstantExpression)ex.Arguments[0]).Value
                     : System.Linq.Expressions.Expression.Lambda(ex.Arguments[0]).Compile().DynamicInvoke();

            string format = tag ? "{0} LIKE {1}" : "{0} NOT LIKE {1}";
            string param = "";
            switch (ex.Method.Name)
            {
                case "StartsWith":
                    param = SetArgument(name, "%" + value);
                    break;
                case "EndsWith":
                    param = SetArgument(name, value + "%");
                    break;
                case "Contains":
                    param = SetArgument(name, "%" + value + "%");
                    break;
                default:
                    throw new Exception("未实现的(Lambda:" + ex.Method.Name + ")表达式！请使用Where条件来操作！");
            }
            return string.Format(format, name, param);
        }

        private string SetArgument(string name, object value)
        {
            var param = "@" + name + Index;
            Parametets[param] = value;
            Index++;
            return param;
        }

        private string ClauseText(BinaryExpression exp, string op)
        {
            var expLeft = exp.Left;
            var expRight = exp.Right;
            //成员
            string name = (expLeft as MemberExpression).Member.Name;
            object value = null;
            if (expRight.NodeType == ExpressionType.Constant)
            {
                value = (expRight as ConstantExpression).Value;
            }
            //Convert->比如：o=>o.DateTime>Convert.ToDateTime("2015-09-09")
            if (expRight.NodeType == ExpressionType.Call)
            {
                value = Expression.Lambda(expRight).Compile().DynamicInvoke();
            }
            //Convert->比如：o=>o.DateTime==DateTime.Now
            if (expRight.NodeType == ExpressionType.MemberAccess)
            {
                value = Expression.Lambda(expRight).Compile().DynamicInvoke();
            }
            string Result = "";
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                if (op == "=")
                {
                    Result = name + " IS NULL";
                }
                if (op == "<>")
                {
                    Result = name + " IS NOT NULL";
                }
            }
            else
            {
                //sql
                Result = string.Format("{0} {1} {2}", name, op, SetArgument(name, value));
            }
            return Result;
        }
    }
}
