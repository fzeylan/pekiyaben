using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PekiYaBen.API.Commands;
using PekiYaBen.API.Helpers;
using PekiYaBen.API.Models.APIModels;
using PekiYaBen.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static PekiYaBen.API.Enums.General;

namespace PekiYaBen.API.Services
{
    public interface IAppUserService
    {
        CommandResponse Authenticate(string email, string password, string ipaddress);
        CommandResponse SocialAuthenticate(string email, string fullName, string profilePhoto, string socialToken, SocialMedia socialMedia, string ipaddress);
        APIResponse VerifyToken(string SocialToken, SocialMedia Media);
        
        APIResponse VerifyPurchase(string token, string productId);
        APIResponse Register(AppUser user);
        APIResponse Update(UdpateUser user, string token);
        APIResponse UpdateProfilePhoto(string token, byte[] profilePhoto);
        APIResponse FCMRegister(string token, string fcmToken);
        APIResponse ChangePassword(string token, ChangePassword pass);
        APIResponse ResetPassword(string email);
    }

    public class AppUserService : IAppUserService
    {
        private readonly AppSettings _appSettings;

        public AppUserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        private string GenerateToken(AppUser user)
        {
            //var registerUser = (AppUser)response.Data;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public CommandResponse Authenticate(string Email, string Password, string IPAddress)
        {

            AppUserCommand command = new AppUserCommand(_appSettings);
            var userCommand = command.Login(Email, Password, IPAddress);

            if (userCommand.Succeed)
            {
                var user = (AppUser)userCommand.Data;
                user.Token = GenerateToken(user);
                command.UpdateToken(user.Id, user.Token);
                user = user.WithoutPassword();
                userCommand.Data = user;
            }
            return userCommand;
        }

        public CommandResponse SocialAuthenticate(string Email, string FullName, string ProfilePhoto, string SocialToken, SocialMedia Media, string IPAddress)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            var userCommand = command.SocialLogin(Email, FullName, ProfilePhoto, SocialToken, Media, IPAddress);

            if (userCommand.Succeed)
            {
                var user = (AppUser)userCommand.Data;
                user.Token = GenerateToken(user);
                command.UpdateToken(user.Id, user.Token);
                user = user.WithoutPassword();
                userCommand.Data = user;
            }
            return userCommand;
        }

        public APIResponse VerifyToken(string SocialToken, SocialMedia Media)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.VerifyToken(SocialToken, Media));
        }

        public APIResponse VerifyPurchase(string token, string productId)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.VerifyPurchase(token, productId));
        }

        public APIResponse Register(AppUser user)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            CommandResponse response = command.Register(user);
            if (response.Succeed)
            {
                var registerUser = (AppUser)response.Data;
                registerUser.Token = GenerateToken(registerUser);

                command.UpdateToken(registerUser.Id, registerUser.Token);

                response.Data = registerUser;
            }

            return new APIResponse(response);
        }

        public APIResponse Update(UdpateUser user, string token)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.Update(user, token));
        }

        public APIResponse UpdateProfilePhoto(string token, byte[] profilePhoto)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.UpdateProfilePhoto(token, profilePhoto));
        }

        public APIResponse FCMRegister(string token, string fcmToken)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.FCMRegister(token, fcmToken));
        }

        public APIResponse ChangePassword(string token, ChangePassword pass)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.ChangePassword(token, pass));
        }

        public APIResponse ResetPassword(string email)
        {
            AppUserCommand command = new AppUserCommand(_appSettings);
            return new APIResponse(command.ResetPassword(email));
        }

    }
}
