using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using System.ComponentModel;

namespace Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Validators
{
    public class FormFluentValidator : ComponentBase, IDisposable
    {
        public static readonly char[] separators = new[] { '.', '[' };
        [Parameter] public IValidator Validator { get; set; }

        [Parameter] public EditContext EditContext { get; set; }
        [CascadingParameter] public EditContext CascadingEditContext { get; set; }

        [Parameter] public Func<EditContext, Task> OnValidSubmit { get; set; }
        [Parameter] public Func<EditContext, Task> OnInvalidSubmit { get; set; }
        bool disablePopup;
        ValidationMessageStore messages;
        static bool isBusy = false;

        protected override void OnInitialized()
        {
            EditContext ??= CascadingEditContext;
            messages = new ValidationMessageStore(EditContext);
            var args = new ValidationRequestedEventArgs();

            // Revalidate when any field changes, or if the entire form requests validation
            // (e.g., on submit)

            EditContext.OnValidationRequested += async (sender, eventArgs)
               => await ValidateModel((EditContext)sender ?? throw new NullReferenceException($"{nameof(sender)} Não pode ser nulo"));

            base.OnInitialized();
        }

        public async Task<bool> ValidateAsync(string? parameterToValidate = null)
        {
            //this.EditContext = new EditContext(this.EditContext.Model);
            //disablePopup = true;
            //return true;
            //this.EditContext.Validate();
            return (await ValidateModelAsync(EditContext, parameterToValidate)).IsValid;
        }

        private async Task<ValidationResult> ValidateModel(EditContext editContext, bool invokeCallback = true)
        {
            //try
            //{
            if (Validator != null)
            {
                ValidationResult validationResult = await ValidateModelAsync(editContext);

                if (validationResult.IsValid)
                    await OnValidSubmit(editContext);
                else
                    await OnInvalidSubmit(editContext);

                return validationResult;
            }
            return new ValidationResult();
            //}
            //catch(Exception ex)
            //{
            //    throw;
            //}
        }

        private async Task<ValidationResult> ValidateModelAsync(EditContext editContext, string? parameterToValidate = null)
        {
            messages.Clear();
            editContext.NotifyValidationStateChanged();
            if (editContext.Model is EntityDTO entity)
            {
                entity.IsSubmiting = true;
            }
            var context = new ValidationContext<object>(editContext.Model);
            var validationResult = await Validator.ValidateAsync(context);
            foreach (var error in validationResult.Errors)
            {
                if (!string.IsNullOrWhiteSpace(parameterToValidate))
                    if (!parameterToValidate.Equals(error.PropertyName, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                var fieldIdentifier = ToFieldIdentifier(editContext, error.PropertyName);

                var attrs = fieldIdentifier.Model?.GetType()?.GetProperty(fieldIdentifier.FieldName)?.GetCustomAttributes(false)?.ToDictionary(a => a?.GetType().Name ?? "", a => a);
                var attributeName = fieldIdentifier.FieldName;
                if (attrs?.TryGetValue("DisplayNameAttribute", out var attr) == true)
                {
                    attributeName = (attr as DisplayNameAttribute)?.DisplayName;
                }
                var name = error.FormattedMessagePlaceholderValues["PropertyName"]?.ToString();
                var newMessage = error.ErrorMessage;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    newMessage = error.ErrorMessage.Replace(name, attributeName);
                }
                messages.Add(fieldIdentifier, newMessage);
            }
            if (editContext.Model is EntityDTO entity2)
            {
                entity2.IsSubmiting = false;
            }
            editContext.NotifyValidationStateChanged();
            return validationResult;
        }

        private static FieldIdentifier ToFieldIdentifier(EditContext editContext, string propertyPath)
        {
            // This method parses property paths like 'SomeProp.MyCollection[123].ChildProp'
            // and returns a FieldIdentifier which is an (instance, propName) pair. For example,
            // it would return the pair (SomeProp.MyCollection[123], "ChildProp"). It traverses
            // as far into the propertyPath as it can go until it finds any null instance.

            var obj = editContext.Model;

            while (true)
            {
                var nextTokenEnd = propertyPath.IndexOfAny(separators);
                if (nextTokenEnd < 0)
                {
                    return new FieldIdentifier(obj, propertyPath);
                }

                var nextToken = propertyPath.Substring(0, nextTokenEnd);
                propertyPath = propertyPath.Substring(nextTokenEnd + 1);

                object newObj;
                if (nextToken.EndsWith("]"))
                {
                    // It's an indexer
                    // This code assumes C# conventions (one indexer named Item with one param)
                    nextToken = nextToken.Substring(0, nextToken.Length - 1);
                    var prop = obj.GetType().GetProperty("Item");
                    var indexerType = prop.GetIndexParameters()[0].ParameterType;
                    var indexerValue = Convert.ChangeType(nextToken, indexerType);
                    newObj = prop.GetValue(obj, new object[] { indexerValue });

                }
                else
                {
                    // It's a regular property
                    var prop = obj.GetType().GetProperty(nextToken);
                    if (prop == null)
                    {
                        throw new InvalidOperationException($"Could not find property named {nextToken} on object of type {obj.GetType().FullName}.");
                    }
                    newObj = prop.GetValue(obj);
                }

                if (newObj == null)
                {
                    // This is as far as we can go
                    return new FieldIdentifier(obj, nextToken);
                }

                obj = newObj;
            }
        }

        public void Dispose()
        {
        }
    }
}
