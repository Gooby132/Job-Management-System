﻿using FluentResults;
using JobManagement.Domain.Common;
using JobManagement.Domain.Users.Errors;

namespace JobManagement.Domain.Users.ValueObjects;

/// <summary>
/// Represents the name of the user
/// </summary>
public class UserName : IValueObject
{

    public const int MaxUserNameLength = 100;
    public const int MinUserNameLength = 2;

    public required string Value { get; init; }

    /// <summary>
    /// Factory Method
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Result<UserName> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return UserErrorFactory.UserNameIsInvalid();

        if (name.Length < MinUserNameLength)
            return UserErrorFactory.UserNameIsTooShort();

        if (name.Length > MaxUserNameLength)
            return UserErrorFactory.UserNameIsTooLong();

        return new UserName { Value = name };
    }

    public static bool operator ==(UserName left, UserName right) => left.Equals(right); 
    public static bool operator !=(UserName left, UserName right) => !left.Equals(right); 

    public override string ToString() => Value;
   
    public override int GetHashCode() => Value.GetHashCode();
    
    public override bool Equals(object? obj) => 
        obj is UserName &&
        ((obj as UserName)!.Value) == Value;

}
