using System.ComponentModel;
using GraphQL.Types;

namespace ServerPart.Models;

[Description("Genders")]
[PascalCase]
public enum GenderEnum
{
    [Description("Male Gender")]
    Male,
    [Description("Female Gender")]
    Female
}
