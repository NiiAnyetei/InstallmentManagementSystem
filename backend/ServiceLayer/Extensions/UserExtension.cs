using DataLayer.Models.Data;
using DataLayer.Models.DTOs;

namespace ServiceLayer.Extensions
{
    public static class UserExtension
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto(user.Username, user.Email, user.Bio, user.Image);
        }
        
        public static User ToUser(this UserDto userDto)
        {
            return new User() { Username = userDto.Username, Email = userDto.Email, Bio = userDto.Bio, Image = userDto.Image };
        }
    }
}
