// Copyright (c) Dmitry Razumikhin, 2016-2019.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace Wiry.Base32
{
    internal sealed class ZBase32Encoding : Base32Encoding
    {
        protected override string Alphabet => "0123456789ABCDEFGHJKMNPQRSTVWXYZ";
        protected override char? PadSymbol => null;
    }
}