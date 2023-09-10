﻿using Company.iFX.Proxy;
using Company.Utility.Encryption.Data;
using Company.Utility.Encryption.Interface;
using ProtoBuf.Grpc;

namespace Company.Utility.Encryption.Service
{
    public class EncryptionUtilityProxy
        : IEncryptionUtility
    {
        private readonly IEncryptionUtility m_Proxy;

        public EncryptionUtilityProxy()
        {
            m_Proxy = Proxy.Create<IEncryptionUtility>();
        }

        public async Task<CreateKeysResponse> CreateKeysAsync(
            CreateKeysRequest request,
            CallContext context = default)
        {
            return await m_Proxy.CreateKeysAsync(request, context);
        }

        public async Task<EncryptResponse> EncryptAsync(
            EncryptRequest request,
            CallContext context = default)
        {
            return await m_Proxy.EncryptAsync(request, context);
        }

        public async Task<DecryptResponse> DecryptAsync(
            DecryptRequest request,
            CallContext context = default)
        {
            return await m_Proxy.DecryptAsync(request, context);
        }

        public async Task<RotateAsymmetricKeyResponse> RotateAsymmetricKeyAsync(
            RotateAsymmetricKeyRequest request,
            CallContext context = default)
        {
            return await m_Proxy.RotateAsymmetricKeyAsync(request, context);
        }

        public async Task<ViewSymmetricKeyDefinitionResponse> ViewSymmetricKeyDefinitionAsync(
            ViewSymmetricKeyDefinitionRequest request,
            CallContext context = default)
        {
            return await m_Proxy.ViewSymmetricKeyDefinitionAsync(request, context);
        }

        public async Task<ViewAsymmetricKeyDefinitionResponse> ViewAsymmetricKeyDefinitionAsync(
            ViewAsymmetricKeyDefinitionRequest request,
            CallContext context = default)
        {
            return await m_Proxy.ViewAsymmetricKeyDefinitionAsync(request, context);
        }
    }
}
