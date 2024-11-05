using DataLayer.Models.DTOs;
using Shared.Utils;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models.Data
{
    public class User
    {
        public User()
        {
        }

        public User(NewUserDto newUser)
        {
            Password = newUser.Password;
            Username = newUser.Username;
            Email = newUser.Email;
        }

        public Guid Id { get; set; } = SequentialGuidGenerator.Instance.Create();

        [MaxLength(100)]
        public string Username { get; set; } = default!;

        [MaxLength(100)]
        public string Email { get; set; } = default!;

        [MaxLength(100)]
        public string Password { get; set; } = default!;

        [MaxLength(300)]
        public string Bio { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Image { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;

        public void UpdateUser(UpdatedUserDto updatedUser)
        {
            //Username = updatedUser.Username ?? Username;
            Email = updatedUser.Email ?? Email;
            Bio = updatedUser.Bio ?? Bio;
            Image = updatedUser.Image ?? Image;
            Password = updatedUser.Password ?? Password;
        }
    }
}
