using Autofac;

using BusinessLogic.Models.Data;
using BusinessLogic.ServiceContracts;
using BusinessLogic.Services.PointServices;

using Exceptions.BusinessLogic;

using NCalc;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BusinessLogic.Services
{
    internal class ConditionParseService : IConditionParseService
    {
        protected MemberLocationService MemberLocationService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<MemberLocationService>();

        private EnterPointService EnterPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<EnterPointService>();

        private CheckPointService CheckPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<CheckPointService>();

        private InteractionPointService InteractionPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<InteractionPointService>();

        private ControlPointService ControlPointService =
            BusinessLogicDependencyHolder.Dependencies.Resolve<ControlPointService>();

        private Regex SubstitutePattern = new Regex("\\{(.+?)\\}");
        private Regex IndexPattern = new Regex("\\[(.+?)\\]");

        public bool ParseCondition(string condition, ActionParseParameters parameters)
        {
            string substituted = SubstituteValues(condition, parameters);
            Expression expression = new Expression(substituted);
            return expression.Evaluate() is bool result && result;
        }

        private string SubstituteValues(string expression, ActionParseParameters values)
        {
            Match match = SubstitutePattern.Match(expression);
            while(match != null && match.Success)
            {
                string chain = SubstituteIndexes(
                    match.Groups[1].Value, values
                );

                string[] chainItems = chain.Split('.');
                object value = GetValue(chainItems, values);

                if (value == null)
                    throw new ExpressionParseException();

                string stringValue = value.ToString().Replace(',', '.');

                expression = SubstitutePattern.Replace(expression, stringValue, 1, match.Index);
                match = SubstitutePattern.Match(expression);
            }

            return expression;
        }

        private string SubstituteIndexes(string chain, ActionParseParameters values)
        {
            Match match = IndexPattern.Match(chain);
            while(match != null && match.Success)
            {
                if (!int.TryParse(match.Groups[1].Value, out int value))
                {
                    object index = SubstituteValues("{" + match.Groups[1].Value + "}", values);

                    if (index == null)
                        throw new ExpressionParseException();

                    chain = IndexPattern.Replace(chain, $"[{index}]", 1, match.Index);
                }

                match = IndexPattern.Match(chain, match.Index + 1);
            }

            return chain;
        }

        private object GetValue(string[] paramChain, ActionParseParameters values)
        {
            object result = GetRootValue(paramChain[0], values);

            for (int i = 1; i < paramChain.Length; ++i)
            {
                if (paramChain[i].Contains("("))
                    result = ExecuteMethod(paramChain[i], result, values);
                else
                    result = GetNextValue(paramChain[i], result, values);
            }

            return result;
        }

        private object GetNextValue(string chainItem, object currentValue, ActionParseParameters values)
        {
            if (currentValue == null)
                return null;

            string propertyName = IndexPattern.Replace(chainItem, "");

            PropertyInfo property = currentValue.GetType().GetProperties()
                .FirstOrDefault(prop => prop.Name == propertyName);

            if (property == null)
                throw new ExpressionParseException();

            return GetIndexed(chainItem, property.GetValue(currentValue), values);
        }

        private object GetRootValue(string chainItem, ActionParseParameters values)
        {
            object result = null;

            string propertyName = IndexPattern.Replace(chainItem, "");

            switch (propertyName)
            {
                case "Location": result = MemberLocationService.Locator[values.Company.Id]; break;
                case "Action": result = values.Action; break;
                case "Room": result = values.Room; break;
                case "Account": result = values.Account; break;
                case "Point": result = values.Point; break;
                case "Role": result = (int)values.Role; break;
                case "Company": result = values.Company; break;
                case "EnterPointData": result = EnterPointService.Data; break;
                case "CheckPointData": result = CheckPointService.Data; break;
                case "InteractionPointData": result = InteractionPointService.Data; break;
                case "ControlPointData": result = ControlPointService.Data; break;
            }

            if (result == null)
                throw new ExpressionParseException();

            return GetIndexed(chainItem, result, values);
        }

        private int[] GetIndexes(string chainItem, ActionParseParameters values)
        {
            List<int> indexes = new List<int>();
            MatchCollection matches = IndexPattern.Matches(chainItem);

            foreach (Match match in matches)
            {
                if (int.TryParse(match.Groups[1].Value, out int value))
                    indexes.Add(value);
            }

            return indexes.ToArray();
        }

        private object GetIndexed(string chainItem, object currentValue, ActionParseParameters values)
        {
            int[] indexes = GetIndexes(chainItem, values);

            if (indexes.Length == 0)
                return currentValue;

            return GetIndexed(currentValue, indexes);
        }

        private object GetIndexed(object currentValue, int[] indexes)
        {
            if (indexes.Length == 0 || currentValue == null)
                return currentValue;

            if (indexes.Count(ind => ind < 0) != 0)
                throw new ExpressionParseException();

            Type type = currentValue.GetType();

            if (type.IsArray)
            {
                Array array = currentValue as Array;

                int[] takenIndexes = indexes.Take(array.Rank).ToArray();
                int[] leftIndexes = indexes.Skip(array.Rank).ToArray();

                object indexed = array.GetValue(takenIndexes);

                return GetIndexed(indexed, leftIndexes);
            }

            if (currentValue is ICollection collection)
            {
                int index = indexes[0];
                int[] leftIndexes = indexes.Skip(1).ToArray();

                if (collection is IDictionary dictionary)
                {
                    if (!dictionary.Contains(index))
                        return null;

                    return GetIndexed(dictionary[index], leftIndexes);
                }

                if (index >= collection.Count)
                    return null;

                object found = collection.OfType<object>().ElementAt(index);
                return GetIndexed(found, leftIndexes);
            }

            throw new ExpressionParseException();
        }

        private object ExecuteMethod(string chainItem, object currentValue, ActionParseParameters values)
        {
            ICollection<object> executedCollection = null;

            if (currentValue == null)
                return null;

            if(!(currentValue is ICollection collection))
                throw new ExpressionParseException();

            if (collection.Count == 0)
                return 0;

            if (collection is IDictionary dictionary)
            {
                executedCollection = new List<object>();
                foreach (DictionaryEntry a in dictionary)
                    executedCollection.Add(a.Value);
            }
            else 
                executedCollection = collection.OfType<object>().ToList();

            string propertyName = IndexPattern.Replace(chainItem, "");
            object result = null;

            switch (propertyName)
            {
                case "Avg()": result = executedCollection.Cast<double>().Average(); break;
                case "Sum()": result = executedCollection.Cast<double>().Sum(); break;
                case "Count()": result = executedCollection.Count(); break;
                case "Max()": result = executedCollection.Max(); break;
                case "Min()": result = executedCollection.Min(); break;
                case "First()": result = executedCollection.First(); break;
                case "Last()": result = executedCollection.Last(); break;
            }

            if (result == null)
                throw new ExpressionParseException();

            return GetIndexed(chainItem, result, values);
        }
    }
}
