using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace LabBilling.Library;

public class SqlParameterConverter
{
    public class ParameterInfo
    {
        public string ColumnName { get; set; }
        public string PlaceholderValue { get; set; }
        public string SqlParameterName { get; set; }
        public BooleanComparisonExpression ComparisonExpression { get; set; }
        public InPredicate InPredicateExpression { get; set; }
        public bool IsList { get; set; }
    }

    public class ParameterVisitor : TSqlFragmentVisitor
    {
        public List<ParameterInfo> FoundParameters { get; } = new();
        private static readonly Regex PlaceholderRegex = new(@"\{([0-2])\}");

        public override void Visit(BooleanComparisonExpression expression)
        {
            if (expression.SecondExpression is StringLiteral literal)
            {
                var match = PlaceholderRegex.Match(literal.Value);
                if (match.Success)
                {
                    string literalValue = match.Groups[1].Value;
                    HandleParameter(expression.FirstExpression, literal.Value, literalValue, expression, null, false);
                }
            }
            base.Visit(expression);
        }

        public override void Visit(BooleanTernaryExpression expression)
        {
            // Ensure it's a BETWEEN expression
            if (expression.TernaryExpressionType == BooleanTernaryExpressionType.Between)
            {
                // Handle BETWEEN clause
                bool modified = false;

                // Check Second Expression (Start Value)
                if (expression.SecondExpression is StringLiteral startLiteral)
                {
                    var match = PlaceholderRegex.Match(startLiteral.Value);
                    if (match.Success)
                    {
                        string literalValue = match.Groups[1].Value;
                        modified |= HandleBetweenParameter(expression, startLiteral, isStart: true, literalValue);
                    }
                }

                // Check Third Expression (End Value)
                if (expression.ThirdExpression is StringLiteral endLiteral)
                {
                    var match = PlaceholderRegex.Match(endLiteral.Value);
                    if (match.Success)
                    {
                        string literalValue = match.Groups[1].Value;
                        modified |= HandleBetweenParameter(expression, endLiteral, isStart: false, literalValue);
                    }
                }

                // If both expressions have been modified, we can proceed
                if (modified)
                {
                    FoundParameters.Add(new ParameterInfo
                    {
                        ColumnName = GetColumnName(expression.FirstExpression),
                        PlaceholderValue = "{0} and {1}",
                        SqlParameterName = "@fromDate and @thruDate",
                        IsList = false
                    });
                }
            }

            base.Visit(expression);
        }

        private bool HandleBetweenParameter(BooleanTernaryExpression expression, StringLiteral literal, bool isStart, string literalValue)
        {
            string paramName;
            switch (literalValue)
            {
                case "0":
                    paramName = "@fromDate";
                    break;
                case "1":
                    paramName = "@thruDate";
                    break;
                default:
                    return false; // Skip other numbers
            }

            // Replace the entire literal with the parameter
            var newParameter = new VariableReference
            {
                Name = paramName
            };

            if (isStart)
            {
                // SecondExpression is the start value in BETWEEN
                expression.SecondExpression = newParameter;
            }
            else
            {
                // ThirdExpression is the end value in BETWEEN
                expression.ThirdExpression = newParameter;
            }

            return true;
        }

        public override void Visit(InPredicate expression)
        {
            // Handle IN clause
            if (expression.Values.Count == 1 && expression.Values[0] is StringLiteral literal)
            {
                var match = PlaceholderRegex.Match(literal.Value);
                if (match.Success)
                {
                    string literalValue = match.Groups[1].Value;
                    HandleParameter(expression.Expression, literal.Value, literalValue, null, expression, true);
                }
            }
            base.Visit(expression);
        }

        private static string FormatParameterName(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
                return string.Empty;

            var result = new StringBuilder();
            bool capitalizeNext = false;
            bool isFirstChar = true;

            foreach (char c in columnName.ToLower())
            {
                if (c == '_')
                {
                    capitalizeNext = true;
                    continue;
                }

                if (isFirstChar)
                {
                    result.Append(c);
                    isFirstChar = false;
                }
                else if (capitalizeNext)
                {
                    result.Append(char.ToUpper(c));
                    capitalizeNext = false;
                }
                else
                {
                    result.Append(c);
                }
            }

            return "@" + result.ToString();
        }

        private void HandleParameter(
            ScalarExpression columnExpression,
            string fullLiteralValue,
            string literalValue,
            BooleanComparisonExpression comparisonExpression,
            InPredicate inPredicate,
            bool isList)
        {
            string paramName;
            switch (literalValue)
            {
                case "0":
                    paramName = "@fromDate";
                    break;
                case "1":
                    paramName = "@thruDate";
                    break;
                case "2":
                    if (columnExpression is ColumnReferenceExpression columnRef)
                    {
                        string simpleColumnName = columnRef.MultiPartIdentifier.Identifiers.Last().Value;
                        string baseParamName = FormatParameterName(simpleColumnName);

                        // Append 'List' if it's an IN clause
                        paramName = isList ? baseParamName + "List" : baseParamName;
                    }
                    else
                    {
                        return; // Skip if not a column reference
                    }
                    break;
                default:
                    return; // Skip other numbers
            }

            // Remove time components
            var timePattern = @"\s+\d{1,2}:\d{2}(:\d{2})?";
            var cleanedValue = Regex.Replace(fullLiteralValue, timePattern, "");

            // Replace the old expression with the new parameter
            var newParameter = new VariableReference
            {
                Name = paramName
            };

            if (comparisonExpression != null)
            {
                comparisonExpression.SecondExpression = newParameter;
            }
            else if (inPredicate != null)
            {
                inPredicate.Values.Clear();
                inPredicate.Values.Add(newParameter);
            }

            string columnName = GetColumnName(columnExpression);

            FoundParameters.Add(new ParameterInfo
            {
                ColumnName = columnName,
                PlaceholderValue = cleanedValue,
                SqlParameterName = paramName,
                ComparisonExpression = comparisonExpression,
                InPredicateExpression = inPredicate,
                IsList = isList
            });
        }

        private string GetColumnName(ScalarExpression expression)
        {
            if (expression is ColumnReferenceExpression colRef)
            {
                return string.Join(".", colRef.MultiPartIdentifier.Identifiers.Select(id => id.Value));
            }
            return string.Empty;
        }
    }


    public static (string ModifiedSql, List<ParameterInfo> Parameters) ConvertToSqlParameters(string sqlScript)
    {
        TSql150Parser parser = new(false);

        using StringReader reader = new(sqlScript);
        TSqlFragment fragment;
        IList<ParseError> errors;
        fragment = parser.Parse(reader, out errors);

        if (errors.Count > 0)
        {
            // Try wrapping unquoted placeholders and parse again
            sqlScript = WrapUnquotedPlaceholders(sqlScript);

            using StringReader fixedReader = new(sqlScript);
            fragment = parser.Parse(fixedReader, out errors);

            if (errors.Count > 0)
            {
                throw new Exception($"SQL parsing error: {string.Join(", ", errors)}");
            }
        }

        var visitor = new ParameterVisitor();
        fragment.Accept(visitor);

        foreach (var param in visitor.FoundParameters)
        {
            var newParameter = new VariableReference
            {
                Name = param.SqlParameterName
            };

            // Replace the old expression with the new parameter
            if (param.ComparisonExpression != null)
            {
                param.ComparisonExpression.SecondExpression = newParameter;
            }
            else if (param.InPredicateExpression != null)
            {
                param.InPredicateExpression.Values.Clear();
                param.InPredicateExpression.Values.Add(newParameter);
            }
        }

        Sql150ScriptGenerator generator = new Sql150ScriptGenerator(
            new SqlScriptGeneratorOptions
            {
                KeywordCasing = KeywordCasing.Uppercase
            });

        string modifiedSql;
        generator.GenerateScript(fragment, out modifiedSql);

        return (modifiedSql, visitor.FoundParameters);
    }

    private static string WrapUnquotedPlaceholders(string sqlScript)
    {
        var regex = new Regex(@"('(?:''|[^'])*')|({\d+})", RegexOptions.Singleline);
        return regex.Replace(sqlScript, m =>
        {
            if (m.Groups[1].Success)
            {
                // Quoted string, return as is
                return m.Value;
            }
            else if (m.Groups[2].Success)
            {
                // Unquoted placeholder, wrap with single quotes
                return $"'{m.Groups[2].Value}'";
            }
            else
            {
                return m.Value;
            }
        });
    }
}
