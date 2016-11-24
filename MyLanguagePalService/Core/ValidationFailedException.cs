using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace MyLanguagePalService.Core
{
    public class ValidationFailedException : Exception
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        public ValidationFailedException([NotNull] string fieldName, [NotNull] string error)
        {
            if (fieldName == null)
                throw new ArgumentNullException(nameof(fieldName));
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            _errors.Add(new ValidationError()
            {
                FieldName = fieldName,
                Error = error
            });
        }

        [NotNull]
        public List<ValidationError> Errors => _errors;
    }
}