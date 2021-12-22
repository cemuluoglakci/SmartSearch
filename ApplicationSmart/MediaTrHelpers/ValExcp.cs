using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace ApplicationSmart.MediaTrHelpers
{
    public class ValExcp : Exception
    {
        private IDictionary<string, string[]> FailureDict;
        public ValExcp()
            : base("Exception detected!")
        {
            FailureDict = new Dictionary<string, string[]>();
        }

        public ValExcp(List<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                FailureDict.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Failures
        {
            get { return FailureDict; }
        }

    }
}
