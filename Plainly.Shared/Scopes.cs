using System.Runtime.Serialization;

namespace Plainly.Shared;

[DataContract]
public enum Scopes
{
    [EnumMember(Value = "User.Create")]
    CreateUser,
    [EnumMember(Value = "User.View")]
    ViewUser,
    [EnumMember(Value = "User.Edit")]
    EditUser,
    [EnumMember(Value = "User.Delete")]
    DeleteUser,
}