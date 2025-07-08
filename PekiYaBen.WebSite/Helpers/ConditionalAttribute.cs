using PekiYaBen.WebSite.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PekiYaBen.WebSite.Enums.General;

namespace PekiYaBen.WebSite.Helpers
{
    public class RequiredForAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string _otherProperty;
        private readonly string _type;
        public RequiredForAttribute(string otherProperty, string type)
        {
            _otherProperty = otherProperty;
            _type = type;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_otherProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format("Unknown property {0}", new[] { _otherProperty }));
            }
            var otherPropertyValue = property.GetValue(validationContext.ObjectInstance, null).ToString();

            if (otherPropertyValue != _type)
            {
                if (value == null || value as string == string.Empty)
                {
                    return new ValidationResult(string.Format(FormatErrorMessage(validationContext.DisplayName), new[] { _otherProperty }));
                }
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredfor",
            };
            rule.ValidationParameters.Add("other", _otherProperty);
            rule.ValidationParameters.Add("type", _type);
            yield return rule;
        }
    }
}