using FluentValidation;
using Krosoft.Extensions.Options.Services;
using Krosoft.Extensions.Samples.DotNet9.Api.Models;

namespace Krosoft.Extensions.Samples.DotNet9.Api.Services;

internal class AppSettingsValidateOptions(IValidator<AppSettings> validator) : SettingsValidateOptions<AppSettings>(validator);

 