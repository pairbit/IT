using IT.Ext;
using IT.Validation;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Security.Cryptography.JinnServer.Internal
{
    internal class SigningService : JinnServerService, ISigningService
    {
        private static readonly Int32 PartInBytes = FileSize.MB.GetBytes(50);

        #region ISigningService

        public String Sign(String data, String alg, SignFormat format, Boolean detached)
        {
            if (!detached) data = Soap.Request.SignMessage.Format(data).ToBase64();

            var request = Soap.Request.Sign.Format(data, format.DisplayName(), alg);

            var response = GetResponse(request, Soap.Actions.Sign);

            return response.SigningResponseType;
        }

        public String Digest(Stream data, String alg)
        {
            //var signedCadesBES = Sign(data, alg, SignFormat.CadesBES, false);
            //var signedXadesBES = Sign(data, alg, SignFormat.XadesBES, false);

            string digest = null;

            var parts = data.GetParts(PartInBytes);
            string state = null;
            for (int part = 0; part < parts; part++)
            {
                var bytes = data.ReadPartBytes(PartInBytes, part);

                var dataBase64 = bytes.ToBase64();

                var request = state == null ? Soap.Request.Digest.Format(dataBase64, alg) :
                                              Soap.Request.DigestPart.Format(dataBase64, alg, state);

                var response = GetResponse(request, Soap.Actions.Digest);

                var digestResponse = response?.DigestResponseType;

                Arg.NotNull(digestResponse, nameof(digestResponse));

                digest = digestResponse.Digest;

                state = digestResponse.State;
            }

            Arg.NotNull(digest, nameof(digest));
            return digest.FromBase64ToHex();
        }

        #endregion ISigningService

        #region ISigningServiceAsync

        public async Task<String> SignAsync(String data, String alg, SignFormat format, Boolean detached, CancellationToken cancellationToken = default)
        {
            if (!detached) data = Soap.Request.SignMessage.Format(data).ToBase64();

            var request = Soap.Request.Sign.Format(data, format.DisplayName(), alg);

            var response = await GetResponseAsync(request, Soap.Actions.Sign).CA();

            return response.SigningResponseType;
        }

        public async Task<String> DigestAsync(Stream data, String alg, CancellationToken cancellationToken = default)
        {
            string digest = null;

            var parts = data.GetParts(PartInBytes);
            string state = null;
            for (int part = 0; part < parts; part++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var bytes = await data.ReadPartBytesAsync(PartInBytes, part).CA();

                var dataBase64 = bytes.ToBase64();

                var request = state == null ? Soap.Request.Digest.Format(dataBase64, alg) :
                                              Soap.Request.DigestPart.Format(dataBase64, alg, state);
                
                cancellationToken.ThrowIfCancellationRequested();
                
                var response = await GetResponseAsync(request, Soap.Actions.Digest).CA();

                var digestResponse = response?.DigestResponseType;

                Arg.NotNull(digestResponse, nameof(digestResponse));

                digest = digestResponse.Digest;

                state = digestResponse.State;
            }

            Arg.NotNull(digest, nameof(digest));
            return digest.FromBase64ToHex();
        }

        #endregion ISigningServiceAsync
    }
}