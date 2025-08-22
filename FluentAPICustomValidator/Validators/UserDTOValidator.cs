using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAPICustomValidator.Data;
using FluentAPICustomValidator.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FluentAPICustomValidator.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        private readonly UserDbContext _dbContext;
        public UserDTOValidator(UserDbContext dbContext)
        {
            _dbContext = dbContext;
            // ----------------------------
            // Xác thực cấp thuộc tính (Property-Level Validations)
            // ----------------------------
            // Xác thực FirstName: không được để trống và chỉ chứa chữ cái.
            RuleFor(user => user.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .Must(name => name.All(char.IsLetter))
                .WithMessage("First Name must contain only letters.");
            // Xác thực LastName: không được để trống và chỉ chứa chữ cái.
            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .Must(name => name.All(char.IsLetter))
                .WithMessage("Last Name must contain only letters.");
            // Xác thực Email: không được để trống, đúng định dạng email và phải là duy nhất.
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be in a valid format.")
                .MustAsync(async (email, cancellationToken) =>
                {
                    // Kiểm tra cơ sở dữ liệu để đảm bảo email là duy nhất.
                    return !await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
                })
                .WithMessage("Email must be unique.");
            // Xác thực Password: không được để trống.
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.");
            // Xác thực PhoneNumber: kiểm tra cơ bản để đảm bảo nó được cung cấp.
            RuleFor(user => user.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.");
            // Xác thực Address: trường tùy chọn, nhưng giới hạn độ dài tối đa nếu được cung cấp.
            RuleFor(user => user.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
            // Xác thực DateOfBirth: không được để trống, không thể là ngày tương lai và người dùng phải đủ 18 tuổi.
            RuleFor(user => user.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of Birth cannot be a future date.")
                .Must(BeAtLeast18YearsOld)
                .WithMessage("User must be at least 18 years old.");
            // Xác thực GenderId: kiểm tra bất đồng bộ rằng Gender được cung cấp có tồn tại.
            RuleFor(user => user.GenderId)
                .MustAsync(IsValidGender)
                .WithMessage("The specified Gender does not exist.");
            // ----------------------------
            // Xác thực cấp đối tượng (Object-Level Validations)
            // ----------------------------
            // Xác thực Password và ConfirmPassword phải khớp nhau.
            RuleFor(user => user)
                .Custom((dto, context) =>
                {
                    if (dto.Password != dto.ConfirmPassword)
                    {
                        // Gán lỗi cho thuộc tính ConfirmPassword.
                        context.AddFailure("ConfirmPassword", "Password and ConfirmPassword must match.");
                    }
                });
            // Xác thực mối quan hệ giữa Country và City:
            // 1. Đảm bảo Country tồn tại.
            // 2. Kiểm tra City đã chỉ định có thuộc về Country đó không.
            RuleFor(user => user)
                .CustomAsync(async (dto, context, cancellationToken) =>
                {
                    // Lấy thông tin quốc gia bao gồm danh sách thành phố.
                    var country = await _dbContext.Countries
                        .Include(c => c.Cities)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CountryId == dto.CountryId, cancellationToken);
                    if (country == null)
                    {
                        context.AddFailure("CountryId", "The selected country does not exist.");
                    }
                    else if (!country.Cities.Any(city => city.CityId == dto.CityId))
                    {
                        context.AddFailure("CityId", $"The selected city does not belong to the country '{country.Name}'.");
                    }
                });
        }
        // Phương thức trợ giúp để kiểm tra người dùng có đủ 18 tuổi hay không.
        private bool BeAtLeast18YearsOld(DateTime dob)
        {
            return dob <= DateTime.Now.AddYears(-18);
        }
        // Phương thức bất đồng bộ để kiểm tra GenderId có tồn tại trong cơ sở dữ liệu không.
        private async Task<bool> IsValidGender(int genderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Genders.AnyAsync(g => g.GenderId == genderId, cancellationToken);
        }
    }

}