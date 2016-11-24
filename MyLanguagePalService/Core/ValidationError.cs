using System;
using JetBrains.Annotations;

namespace MyLanguagePalService.Core
{
    public class ValidationError
    {
        private string _fieldName = string.Empty;
        private string _error = string.Empty;

        [NotNull]
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(FieldName));
                _fieldName = value;
            }
        }

        [NotNull]
        public string Error
        {
            get { return _error; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Error));

                _error = value;
            }
        }
    }
}