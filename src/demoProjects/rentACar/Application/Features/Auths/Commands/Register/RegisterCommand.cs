using Application.Features.Auths.Dtos;
using Application.Features.Auths.Rules;
using Application.Services.AuthServices;
using Application.Services.Repositories;
using AutoMapper;
using Core.Security.Dtos;
using Core.Security.Entities;
using Core.Security.Hashing;
using MediatR;

namespace Application.Features.Auths.Commands.Register;

public class RegisterCommand : IRequest<RegisteredDto>
{
    public UserForRegisterDto UserForRegisterDto { get; set; }
    public string IpAddress { get; set; }

    public class RegisterCommandRequestHandler : IRequestHandler<RegisterCommand, RegisteredDto>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public RegisterCommandRequestHandler(AuthBusinessRules authBusinessRules, IUserRepository userRepository, IAuthService authService)
        {
            _authBusinessRules = authBusinessRules;
            _userRepository = userRepository;
            _authService = authService;
        }
        
        public async Task<RegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.EmailCanNotBeDuplicatedWhenRegistered(request.UserForRegisterDto.Email);

            HashingHelper.CreatePasswordHash(request.UserForRegisterDto.Password, out var passwordHash, out var passwordSalt);

            var newUser = new User()
            {
                FirstName = request.UserForRegisterDto.FirstName,
                LastName = request.UserForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.UserForRegisterDto.Email,
                Status = true
            };

            var createdUser = await _userRepository.AddAsync(newUser);
            
            var createdAccessToken = await _authService.CreateAccessToken(createdUser);
            var createdRefreshToken = await _authService.CreateRefreshToken(createdUser, request.IpAddress);
            var addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            var registeredDto = new RegisteredDto()
            {
                RefreshToken = addedRefreshToken,
                AccessToken = createdAccessToken,
            };

            return registeredDto;
        }
    }
}