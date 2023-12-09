using blog_backend.DAO.Model;
using blog_backend.Entity;

namespace blog_backend.Service.Mappers;

public abstract class EditDtoMapper
{
    public static User Map(EditAccountDTO editAccountDto, User existingUser)
    {
        existingUser.FullName = editAccountDto.FullName;
        existingUser.Email = editAccountDto.Email;
        existingUser.Gender = editAccountDto.Gender;
        existingUser.PhoneNumber = editAccountDto.PhoneNumber;
        existingUser.DateOfBirth = editAccountDto.BirthDate;

        return existingUser;
    }
}