using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Minimal_API.Models
{
    public class ValidationHelper
    {
         // Hàm validate model theo Data Annotation
        public static bool TryValidate<T>(T model, out List<ValidationResult> validationResults)
        {
            validationResults = new List<ValidationResult>();

            // Null check
            if (model == null)
            {
                validationResults.Add(new ValidationResult("Model instance cannot be null."));
                return false;
            }

            // ValidationContext chứa metadata về model cần kiểm tra
            var validationContext = new ValidationContext(model);

            // Thực thi validation trên toàn bộ property được gắn attribute
            return Validator.TryValidateObject(model, validationContext, validationResults, true);
        }
    }
}