﻿using ProtoBuf;

namespace Company.Access.User.Data.Mobile
{
    [ProtoContract]
    [Serializable]
    public class RegisterResponse
        : RegisterResponseBase
    {
        [ProtoMember(1)]
        public required string MobileMessage { get; set; }
    }
}
