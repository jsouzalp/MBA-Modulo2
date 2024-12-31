﻿using FinPlanner360.Business.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinPlanner360.Business.Services
{
    public class AppIdentityUser : IAppIdentityUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AppIdentityUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid GetUserId()
        {
            if (!IsAuthenticated()) return Guid.Empty;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claim))
                claim = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return claim is null ? Guid.Empty : Guid.Parse(claim);
        }

        public string GetUserEmail()
        {
            if (!IsAuthenticated()) return string.Empty;

            var claim = _accessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(claim))
                claim = _accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            return claim is null ? string.Empty : claim;
        }

        //public string GetUsername()
        //{
        //    var username = _accessor.HttpContext?.User.FindFirst("username")?.Value;
        //    if (!string.IsNullOrEmpty(username)) return username;

        //    username = _accessor.HttpContext?.User.Identity?.Name;
        //    if (!string.IsNullOrEmpty(username)) return username;

        //    username = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.Name)?.Value;
        //    if (!string.IsNullOrEmpty(username)) return username;

        //    username = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.GivenName)?.Value;
        //    if (!string.IsNullOrEmpty(username)) return username;

        //    var sub = _accessor.HttpContext?.User.FindFirst(JwtClaimTypes.Subject);
        //    if (sub != null) return sub.Value;

        //    return string.Empty;
        //}

        public bool IsAuthenticated()
        {
            return _accessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }

        //public bool IsInRole(string role)
        //{
        //    return _accessor.HttpContext != null && _accessor.HttpContext.User.IsInRole(role);
        //}

        //public string GetLocalIpAddress()
        //{
        //    return _accessor.HttpContext?.Connection.LocalIpAddress?.ToString();
        //}

        //public string GetRemoteIpAddress()
        //{
        //    return _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        //}
    }
}
