﻿using DataLayer.Models.Data;
using RTools_NTS.Util;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.DTOs;

public record NewUserDto([Required] string Username, [Required] string Email, [Required] string Password);

public record LoginUserDto([Required] string Email, [Required] string Password);

public record LoginUserResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, string Username);
public record NewRefreshTokenResponseDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, string Username);

public record NewRefreshTokenDto([Required] string AccessToken, [Required] string RefreshToken);

/// <summary>
///     At lease one of the items should not be null
/// </summary>
public record UpdatedUserDto(string? Username, string? Email, string? Bio, string? Image, string? Password) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Email) &&
            string.IsNullOrWhiteSpace(Bio) && string.IsNullOrWhiteSpace(Image) && string.IsNullOrWhiteSpace(Password))
        {
            yield return new ValidationResult(
                $"At least on of the fields: {nameof(Username)}, {nameof(Email)}, {nameof(Bio)}, {nameof(Image)}, {nameof(Password)} must be filled"
            );
        }
    }
}

public record UserDto(string Username, string Email, string Bio, string Image);
