#if SPAN
using System;
#endif

namespace HexMate
{
    internal static class ScalarConstants
    {
        // Invalid characters are represented as 0xFF, to be ignored characters as 0xFE
#if SPAN
        internal static ReadOnlySpan<byte> LookupUpperLower => new byte[]
#else
        internal static readonly byte[] LookupUpperLower =
#endif
        {
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFE, 0xFE, 0xFF, 0xFF, 0xFE, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFE, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
        };

        internal static readonly ushort[] s_utf8LookupHexUpperBE =
        {
            0x3030, 0x3031, 0x3032, 0x3033, 0x3034, 0x3035, 0x3036, 0x3037, 0x3038, 0x3039, 0x3041, 0x3042, 0x3043, 0x3044, 0x3045, 0x3046,
            0x3130, 0x3131, 0x3132, 0x3133, 0x3134, 0x3135, 0x3136, 0x3137, 0x3138, 0x3139, 0x3141, 0x3142, 0x3143, 0x3144, 0x3145, 0x3146,
            0x3230, 0x3231, 0x3232, 0x3233, 0x3234, 0x3235, 0x3236, 0x3237, 0x3238, 0x3239, 0x3241, 0x3242, 0x3243, 0x3244, 0x3245, 0x3246,
            0x3330, 0x3331, 0x3332, 0x3333, 0x3334, 0x3335, 0x3336, 0x3337, 0x3338, 0x3339, 0x3341, 0x3342, 0x3343, 0x3344, 0x3345, 0x3346,
            0x3430, 0x3431, 0x3432, 0x3433, 0x3434, 0x3435, 0x3436, 0x3437, 0x3438, 0x3439, 0x3441, 0x3442, 0x3443, 0x3444, 0x3445, 0x3446,
            0x3530, 0x3531, 0x3532, 0x3533, 0x3534, 0x3535, 0x3536, 0x3537, 0x3538, 0x3539, 0x3541, 0x3542, 0x3543, 0x3544, 0x3545, 0x3546,
            0x3630, 0x3631, 0x3632, 0x3633, 0x3634, 0x3635, 0x3636, 0x3637, 0x3638, 0x3639, 0x3641, 0x3642, 0x3643, 0x3644, 0x3645, 0x3646,
            0x3730, 0x3731, 0x3732, 0x3733, 0x3734, 0x3735, 0x3736, 0x3737, 0x3738, 0x3739, 0x3741, 0x3742, 0x3743, 0x3744, 0x3745, 0x3746,
            0x3830, 0x3831, 0x3832, 0x3833, 0x3834, 0x3835, 0x3836, 0x3837, 0x3838, 0x3839, 0x3841, 0x3842, 0x3843, 0x3844, 0x3845, 0x3846,
            0x3930, 0x3931, 0x3932, 0x3933, 0x3934, 0x3935, 0x3936, 0x3937, 0x3938, 0x3939, 0x3941, 0x3942, 0x3943, 0x3944, 0x3945, 0x3946,
            0x4130, 0x4131, 0x4132, 0x4133, 0x4134, 0x4135, 0x4136, 0x4137, 0x4138, 0x4139, 0x4141, 0x4142, 0x4143, 0x4144, 0x4145, 0x4146,
            0x4230, 0x4231, 0x4232, 0x4233, 0x4234, 0x4235, 0x4236, 0x4237, 0x4238, 0x4239, 0x4241, 0x4242, 0x4243, 0x4244, 0x4245, 0x4246,
            0x4330, 0x4331, 0x4332, 0x4333, 0x4334, 0x4335, 0x4336, 0x4337, 0x4338, 0x4339, 0x4341, 0x4342, 0x4343, 0x4344, 0x4345, 0x4346,
            0x4430, 0x4431, 0x4432, 0x4433, 0x4434, 0x4435, 0x4436, 0x4437, 0x4438, 0x4439, 0x4441, 0x4442, 0x4443, 0x4444, 0x4445, 0x4446,
            0x4530, 0x4531, 0x4532, 0x4533, 0x4534, 0x4535, 0x4536, 0x4537, 0x4538, 0x4539, 0x4541, 0x4542, 0x4543, 0x4544, 0x4545, 0x4546,
            0x4630, 0x4631, 0x4632, 0x4633, 0x4634, 0x4635, 0x4636, 0x4637, 0x4638, 0x4639, 0x4641, 0x4642, 0x4643, 0x4644, 0x4645, 0x4646
        };

        internal static readonly ushort[] s_utf8LookupHexUpperLE =
        {
            0x3030, 0x3130, 0x3230, 0x3330, 0x3430, 0x3530, 0x3630, 0x3730, 0x3830, 0x3930, 0x4130, 0x4230, 0x4330, 0x4430, 0x4530, 0x4630,
            0x3031, 0x3131, 0x3231, 0x3331, 0x3431, 0x3531, 0x3631, 0x3731, 0x3831, 0x3931, 0x4131, 0x4231, 0x4331, 0x4431, 0x4531, 0x4631,
            0x3032, 0x3132, 0x3232, 0x3332, 0x3432, 0x3532, 0x3632, 0x3732, 0x3832, 0x3932, 0x4132, 0x4232, 0x4332, 0x4432, 0x4532, 0x4632,
            0x3033, 0x3133, 0x3233, 0x3333, 0x3433, 0x3533, 0x3633, 0x3733, 0x3833, 0x3933, 0x4133, 0x4233, 0x4333, 0x4433, 0x4533, 0x4633,
            0x3034, 0x3134, 0x3234, 0x3334, 0x3434, 0x3534, 0x3634, 0x3734, 0x3834, 0x3934, 0x4134, 0x4234, 0x4334, 0x4434, 0x4534, 0x4634,
            0x3035, 0x3135, 0x3235, 0x3335, 0x3435, 0x3535, 0x3635, 0x3735, 0x3835, 0x3935, 0x4135, 0x4235, 0x4335, 0x4435, 0x4535, 0x4635,
            0x3036, 0x3136, 0x3236, 0x3336, 0x3436, 0x3536, 0x3636, 0x3736, 0x3836, 0x3936, 0x4136, 0x4236, 0x4336, 0x4436, 0x4536, 0x4636,
            0x3037, 0x3137, 0x3237, 0x3337, 0x3437, 0x3537, 0x3637, 0x3737, 0x3837, 0x3937, 0x4137, 0x4237, 0x4337, 0x4437, 0x4537, 0x4637,
            0x3038, 0x3138, 0x3238, 0x3338, 0x3438, 0x3538, 0x3638, 0x3738, 0x3838, 0x3938, 0x4138, 0x4238, 0x4338, 0x4438, 0x4538, 0x4638,
            0x3039, 0x3139, 0x3239, 0x3339, 0x3439, 0x3539, 0x3639, 0x3739, 0x3839, 0x3939, 0x4139, 0x4239, 0x4339, 0x4439, 0x4539, 0x4639,
            0x3041, 0x3141, 0x3241, 0x3341, 0x3441, 0x3541, 0x3641, 0x3741, 0x3841, 0x3941, 0x4141, 0x4241, 0x4341, 0x4441, 0x4541, 0x4641,
            0x3042, 0x3142, 0x3242, 0x3342, 0x3442, 0x3542, 0x3642, 0x3742, 0x3842, 0x3942, 0x4142, 0x4242, 0x4342, 0x4442, 0x4542, 0x4642,
            0x3043, 0x3143, 0x3243, 0x3343, 0x3443, 0x3543, 0x3643, 0x3743, 0x3843, 0x3943, 0x4143, 0x4243, 0x4343, 0x4443, 0x4543, 0x4643,
            0x3044, 0x3144, 0x3244, 0x3344, 0x3444, 0x3544, 0x3644, 0x3744, 0x3844, 0x3944, 0x4144, 0x4244, 0x4344, 0x4444, 0x4544, 0x4644,
            0x3045, 0x3145, 0x3245, 0x3345, 0x3445, 0x3545, 0x3645, 0x3745, 0x3845, 0x3945, 0x4145, 0x4245, 0x4345, 0x4445, 0x4545, 0x4645,
            0x3046, 0x3146, 0x3246, 0x3346, 0x3446, 0x3546, 0x3646, 0x3746, 0x3846, 0x3946, 0x4146, 0x4246, 0x4346, 0x4446, 0x4546, 0x4646
        };

        internal static readonly ushort[] s_utf8LookupHexLowerBE =
        {
            0x3030, 0x3031, 0x3032, 0x3033, 0x3034, 0x3035, 0x3036, 0x3037, 0x3038, 0x3039, 0x3061, 0x3062, 0x3063, 0x3064, 0x3065, 0x3066,
            0x3130, 0x3131, 0x3132, 0x3133, 0x3134, 0x3135, 0x3136, 0x3137, 0x3138, 0x3139, 0x3161, 0x3162, 0x3163, 0x3164, 0x3165, 0x3166,
            0x3230, 0x3231, 0x3232, 0x3233, 0x3234, 0x3235, 0x3236, 0x3237, 0x3238, 0x3239, 0x3261, 0x3262, 0x3263, 0x3264, 0x3265, 0x3266,
            0x3330, 0x3331, 0x3332, 0x3333, 0x3334, 0x3335, 0x3336, 0x3337, 0x3338, 0x3339, 0x3361, 0x3362, 0x3363, 0x3364, 0x3365, 0x3366,
            0x3430, 0x3431, 0x3432, 0x3433, 0x3434, 0x3435, 0x3436, 0x3437, 0x3438, 0x3439, 0x3461, 0x3462, 0x3463, 0x3464, 0x3465, 0x3466,
            0x3530, 0x3531, 0x3532, 0x3533, 0x3534, 0x3535, 0x3536, 0x3537, 0x3538, 0x3539, 0x3561, 0x3562, 0x3563, 0x3564, 0x3565, 0x3566,
            0x3630, 0x3631, 0x3632, 0x3633, 0x3634, 0x3635, 0x3636, 0x3637, 0x3638, 0x3639, 0x3661, 0x3662, 0x3663, 0x3664, 0x3665, 0x3666,
            0x3730, 0x3731, 0x3732, 0x3733, 0x3734, 0x3735, 0x3736, 0x3737, 0x3738, 0x3739, 0x3761, 0x3762, 0x3763, 0x3764, 0x3765, 0x3766,
            0x3830, 0x3831, 0x3832, 0x3833, 0x3834, 0x3835, 0x3836, 0x3837, 0x3838, 0x3839, 0x3861, 0x3862, 0x3863, 0x3864, 0x3865, 0x3866,
            0x3930, 0x3931, 0x3932, 0x3933, 0x3934, 0x3935, 0x3936, 0x3937, 0x3938, 0x3939, 0x3961, 0x3962, 0x3963, 0x3964, 0x3965, 0x3966,
            0x6130, 0x6131, 0x6132, 0x6133, 0x6134, 0x6135, 0x6136, 0x6137, 0x6138, 0x6139, 0x6161, 0x6162, 0x6163, 0x6164, 0x6165, 0x6166,
            0x6230, 0x6231, 0x6232, 0x6233, 0x6234, 0x6235, 0x6236, 0x6237, 0x6238, 0x6239, 0x6261, 0x6262, 0x6263, 0x6264, 0x6265, 0x6266,
            0x6330, 0x6331, 0x6332, 0x6333, 0x6334, 0x6335, 0x6336, 0x6337, 0x6338, 0x6339, 0x6361, 0x6362, 0x6363, 0x6364, 0x6365, 0x6366,
            0x6430, 0x6431, 0x6432, 0x6433, 0x6434, 0x6435, 0x6436, 0x6437, 0x6438, 0x6439, 0x6461, 0x6462, 0x6463, 0x6464, 0x6465, 0x6466,
            0x6530, 0x6531, 0x6532, 0x6533, 0x6534, 0x6535, 0x6536, 0x6537, 0x6538, 0x6539, 0x6561, 0x6562, 0x6563, 0x6564, 0x6565, 0x6566,
            0x6630, 0x6631, 0x6632, 0x6633, 0x6634, 0x6635, 0x6636, 0x6637, 0x6638, 0x6639, 0x6661, 0x6662, 0x6663, 0x6664, 0x6665, 0x6666
        };

        internal static readonly ushort[] s_utf8LookupHexLowerLE =
        {
            0x3030, 0x3130, 0x3230, 0x3330, 0x3430, 0x3530, 0x3630, 0x3730, 0x3830, 0x3930, 0x6130, 0x6230, 0x6330, 0x6430, 0x6530, 0x6630,
            0x3031, 0x3131, 0x3231, 0x3331, 0x3431, 0x3531, 0x3631, 0x3731, 0x3831, 0x3931, 0x6131, 0x6231, 0x6331, 0x6431, 0x6531, 0x6631,
            0x3032, 0x3132, 0x3232, 0x3332, 0x3432, 0x3532, 0x3632, 0x3732, 0x3832, 0x3932, 0x6132, 0x6232, 0x6332, 0x6432, 0x6532, 0x6632,
            0x3033, 0x3133, 0x3233, 0x3333, 0x3433, 0x3533, 0x3633, 0x3733, 0x3833, 0x3933, 0x6133, 0x6233, 0x6333, 0x6433, 0x6533, 0x6633,
            0x3034, 0x3134, 0x3234, 0x3334, 0x3434, 0x3534, 0x3634, 0x3734, 0x3834, 0x3934, 0x6134, 0x6234, 0x6334, 0x6434, 0x6534, 0x6634,
            0x3035, 0x3135, 0x3235, 0x3335, 0x3435, 0x3535, 0x3635, 0x3735, 0x3835, 0x3935, 0x6135, 0x6235, 0x6335, 0x6435, 0x6535, 0x6635,
            0x3036, 0x3136, 0x3236, 0x3336, 0x3436, 0x3536, 0x3636, 0x3736, 0x3836, 0x3936, 0x6136, 0x6236, 0x6336, 0x6436, 0x6536, 0x6636,
            0x3037, 0x3137, 0x3237, 0x3337, 0x3437, 0x3537, 0x3637, 0x3737, 0x3837, 0x3937, 0x6137, 0x6237, 0x6337, 0x6437, 0x6537, 0x6637,
            0x3038, 0x3138, 0x3238, 0x3338, 0x3438, 0x3538, 0x3638, 0x3738, 0x3838, 0x3938, 0x6138, 0x6238, 0x6338, 0x6438, 0x6538, 0x6638,
            0x3039, 0x3139, 0x3239, 0x3339, 0x3439, 0x3539, 0x3639, 0x3739, 0x3839, 0x3939, 0x6139, 0x6239, 0x6339, 0x6439, 0x6539, 0x6639,
            0x3061, 0x3161, 0x3261, 0x3361, 0x3461, 0x3561, 0x3661, 0x3761, 0x3861, 0x3961, 0x6161, 0x6261, 0x6361, 0x6461, 0x6561, 0x6661,
            0x3062, 0x3162, 0x3262, 0x3362, 0x3462, 0x3562, 0x3662, 0x3762, 0x3862, 0x3962, 0x6162, 0x6262, 0x6362, 0x6462, 0x6562, 0x6662,
            0x3063, 0x3163, 0x3263, 0x3363, 0x3463, 0x3563, 0x3663, 0x3763, 0x3863, 0x3963, 0x6163, 0x6263, 0x6363, 0x6463, 0x6563, 0x6663,
            0x3064, 0x3164, 0x3264, 0x3364, 0x3464, 0x3564, 0x3664, 0x3764, 0x3864, 0x3964, 0x6164, 0x6264, 0x6364, 0x6464, 0x6564, 0x6664,
            0x3065, 0x3165, 0x3265, 0x3365, 0x3465, 0x3565, 0x3665, 0x3765, 0x3865, 0x3965, 0x6165, 0x6265, 0x6365, 0x6465, 0x6565, 0x6665,
            0x3066, 0x3166, 0x3266, 0x3366, 0x3466, 0x3566, 0x3666, 0x3766, 0x3866, 0x3966, 0x6166, 0x6266, 0x6366, 0x6466, 0x6566, 0x6666
        };

        internal static readonly uint[] s_utf16LookupHexUpperBE =
        {
            0x00300030, 0x00300031, 0x00300032, 0x00300033, 0x00300034, 0x00300035, 0x00300036, 0x00300037, 0x00300038, 0x00300039, 0x00300041, 0x00300042, 0x00300043, 0x00300044, 0x00300045, 0x00300046,
            0x00310030, 0x00310031, 0x00310032, 0x00310033, 0x00310034, 0x00310035, 0x00310036, 0x00310037, 0x00310038, 0x00310039, 0x00310041, 0x00310042, 0x00310043, 0x00310044, 0x00310045, 0x00310046,
            0x00320030, 0x00320031, 0x00320032, 0x00320033, 0x00320034, 0x00320035, 0x00320036, 0x00320037, 0x00320038, 0x00320039, 0x00320041, 0x00320042, 0x00320043, 0x00320044, 0x00320045, 0x00320046,
            0x00330030, 0x00330031, 0x00330032, 0x00330033, 0x00330034, 0x00330035, 0x00330036, 0x00330037, 0x00330038, 0x00330039, 0x00330041, 0x00330042, 0x00330043, 0x00330044, 0x00330045, 0x00330046,
            0x00340030, 0x00340031, 0x00340032, 0x00340033, 0x00340034, 0x00340035, 0x00340036, 0x00340037, 0x00340038, 0x00340039, 0x00340041, 0x00340042, 0x00340043, 0x00340044, 0x00340045, 0x00340046,
            0x00350030, 0x00350031, 0x00350032, 0x00350033, 0x00350034, 0x00350035, 0x00350036, 0x00350037, 0x00350038, 0x00350039, 0x00350041, 0x00350042, 0x00350043, 0x00350044, 0x00350045, 0x00350046,
            0x00360030, 0x00360031, 0x00360032, 0x00360033, 0x00360034, 0x00360035, 0x00360036, 0x00360037, 0x00360038, 0x00360039, 0x00360041, 0x00360042, 0x00360043, 0x00360044, 0x00360045, 0x00360046,
            0x00370030, 0x00370031, 0x00370032, 0x00370033, 0x00370034, 0x00370035, 0x00370036, 0x00370037, 0x00370038, 0x00370039, 0x00370041, 0x00370042, 0x00370043, 0x00370044, 0x00370045, 0x00370046,
            0x00380030, 0x00380031, 0x00380032, 0x00380033, 0x00380034, 0x00380035, 0x00380036, 0x00380037, 0x00380038, 0x00380039, 0x00380041, 0x00380042, 0x00380043, 0x00380044, 0x00380045, 0x00380046,
            0x00390030, 0x00390031, 0x00390032, 0x00390033, 0x00390034, 0x00390035, 0x00390036, 0x00390037, 0x00390038, 0x00390039, 0x00390041, 0x00390042, 0x00390043, 0x00390044, 0x00390045, 0x00390046,
            0x00410030, 0x00410031, 0x00410032, 0x00410033, 0x00410034, 0x00410035, 0x00410036, 0x00410037, 0x00410038, 0x00410039, 0x00410041, 0x00410042, 0x00410043, 0x00410044, 0x00410045, 0x00410046,
            0x00420030, 0x00420031, 0x00420032, 0x00420033, 0x00420034, 0x00420035, 0x00420036, 0x00420037, 0x00420038, 0x00420039, 0x00420041, 0x00420042, 0x00420043, 0x00420044, 0x00420045, 0x00420046,
            0x00430030, 0x00430031, 0x00430032, 0x00430033, 0x00430034, 0x00430035, 0x00430036, 0x00430037, 0x00430038, 0x00430039, 0x00430041, 0x00430042, 0x00430043, 0x00430044, 0x00430045, 0x00430046,
            0x00440030, 0x00440031, 0x00440032, 0x00440033, 0x00440034, 0x00440035, 0x00440036, 0x00440037, 0x00440038, 0x00440039, 0x00440041, 0x00440042, 0x00440043, 0x00440044, 0x00440045, 0x00440046,
            0x00450030, 0x00450031, 0x00450032, 0x00450033, 0x00450034, 0x00450035, 0x00450036, 0x00450037, 0x00450038, 0x00450039, 0x00450041, 0x00450042, 0x00450043, 0x00450044, 0x00450045, 0x00450046,
            0x00460030, 0x00460031, 0x00460032, 0x00460033, 0x00460034, 0x00460035, 0x00460036, 0x00460037, 0x00460038, 0x00460039, 0x00460041, 0x00460042, 0x00460043, 0x00460044, 0x00460045, 0x00460046
        };

        internal static readonly uint[] s_utf16LookupHexUpperLE =
        {
            0x00300030, 0x00310030, 0x00320030, 0x00330030, 0x00340030, 0x00350030, 0x00360030, 0x00370030, 0x00380030, 0x00390030, 0x00410030, 0x00420030, 0x00430030, 0x00440030, 0x00450030, 0x00460030,
            0x00300031, 0x00310031, 0x00320031, 0x00330031, 0x00340031, 0x00350031, 0x00360031, 0x00370031, 0x00380031, 0x00390031, 0x00410031, 0x00420031, 0x00430031, 0x00440031, 0x00450031, 0x00460031,
            0x00300032, 0x00310032, 0x00320032, 0x00330032, 0x00340032, 0x00350032, 0x00360032, 0x00370032, 0x00380032, 0x00390032, 0x00410032, 0x00420032, 0x00430032, 0x00440032, 0x00450032, 0x00460032,
            0x00300033, 0x00310033, 0x00320033, 0x00330033, 0x00340033, 0x00350033, 0x00360033, 0x00370033, 0x00380033, 0x00390033, 0x00410033, 0x00420033, 0x00430033, 0x00440033, 0x00450033, 0x00460033,
            0x00300034, 0x00310034, 0x00320034, 0x00330034, 0x00340034, 0x00350034, 0x00360034, 0x00370034, 0x00380034, 0x00390034, 0x00410034, 0x00420034, 0x00430034, 0x00440034, 0x00450034, 0x00460034,
            0x00300035, 0x00310035, 0x00320035, 0x00330035, 0x00340035, 0x00350035, 0x00360035, 0x00370035, 0x00380035, 0x00390035, 0x00410035, 0x00420035, 0x00430035, 0x00440035, 0x00450035, 0x00460035,
            0x00300036, 0x00310036, 0x00320036, 0x00330036, 0x00340036, 0x00350036, 0x00360036, 0x00370036, 0x00380036, 0x00390036, 0x00410036, 0x00420036, 0x00430036, 0x00440036, 0x00450036, 0x00460036,
            0x00300037, 0x00310037, 0x00320037, 0x00330037, 0x00340037, 0x00350037, 0x00360037, 0x00370037, 0x00380037, 0x00390037, 0x00410037, 0x00420037, 0x00430037, 0x00440037, 0x00450037, 0x00460037,
            0x00300038, 0x00310038, 0x00320038, 0x00330038, 0x00340038, 0x00350038, 0x00360038, 0x00370038, 0x00380038, 0x00390038, 0x00410038, 0x00420038, 0x00430038, 0x00440038, 0x00450038, 0x00460038,
            0x00300039, 0x00310039, 0x00320039, 0x00330039, 0x00340039, 0x00350039, 0x00360039, 0x00370039, 0x00380039, 0x00390039, 0x00410039, 0x00420039, 0x00430039, 0x00440039, 0x00450039, 0x00460039,
            0x00300041, 0x00310041, 0x00320041, 0x00330041, 0x00340041, 0x00350041, 0x00360041, 0x00370041, 0x00380041, 0x00390041, 0x00410041, 0x00420041, 0x00430041, 0x00440041, 0x00450041, 0x00460041,
            0x00300042, 0x00310042, 0x00320042, 0x00330042, 0x00340042, 0x00350042, 0x00360042, 0x00370042, 0x00380042, 0x00390042, 0x00410042, 0x00420042, 0x00430042, 0x00440042, 0x00450042, 0x00460042,
            0x00300043, 0x00310043, 0x00320043, 0x00330043, 0x00340043, 0x00350043, 0x00360043, 0x00370043, 0x00380043, 0x00390043, 0x00410043, 0x00420043, 0x00430043, 0x00440043, 0x00450043, 0x00460043,
            0x00300044, 0x00310044, 0x00320044, 0x00330044, 0x00340044, 0x00350044, 0x00360044, 0x00370044, 0x00380044, 0x00390044, 0x00410044, 0x00420044, 0x00430044, 0x00440044, 0x00450044, 0x00460044,
            0x00300045, 0x00310045, 0x00320045, 0x00330045, 0x00340045, 0x00350045, 0x00360045, 0x00370045, 0x00380045, 0x00390045, 0x00410045, 0x00420045, 0x00430045, 0x00440045, 0x00450045, 0x00460045,
            0x00300046, 0x00310046, 0x00320046, 0x00330046, 0x00340046, 0x00350046, 0x00360046, 0x00370046, 0x00380046, 0x00390046, 0x00410046, 0x00420046, 0x00430046, 0x00440046, 0x00450046, 0x00460046
        };

        internal static readonly uint[] s_utf16LookupHexLowerBE =
        {
            0x00300030, 0x00300031, 0x00300032, 0x00300033, 0x00300034, 0x00300035, 0x00300036, 0x00300037, 0x00300038, 0x00300039, 0x00300061, 0x00300062, 0x00300063, 0x00300064, 0x00300065, 0x00300066,
            0x00310030, 0x00310031, 0x00310032, 0x00310033, 0x00310034, 0x00310035, 0x00310036, 0x00310037, 0x00310038, 0x00310039, 0x00310061, 0x00310062, 0x00310063, 0x00310064, 0x00310065, 0x00310066,
            0x00320030, 0x00320031, 0x00320032, 0x00320033, 0x00320034, 0x00320035, 0x00320036, 0x00320037, 0x00320038, 0x00320039, 0x00320061, 0x00320062, 0x00320063, 0x00320064, 0x00320065, 0x00320066,
            0x00330030, 0x00330031, 0x00330032, 0x00330033, 0x00330034, 0x00330035, 0x00330036, 0x00330037, 0x00330038, 0x00330039, 0x00330061, 0x00330062, 0x00330063, 0x00330064, 0x00330065, 0x00330066,
            0x00340030, 0x00340031, 0x00340032, 0x00340033, 0x00340034, 0x00340035, 0x00340036, 0x00340037, 0x00340038, 0x00340039, 0x00340061, 0x00340062, 0x00340063, 0x00340064, 0x00340065, 0x00340066,
            0x00350030, 0x00350031, 0x00350032, 0x00350033, 0x00350034, 0x00350035, 0x00350036, 0x00350037, 0x00350038, 0x00350039, 0x00350061, 0x00350062, 0x00350063, 0x00350064, 0x00350065, 0x00350066,
            0x00360030, 0x00360031, 0x00360032, 0x00360033, 0x00360034, 0x00360035, 0x00360036, 0x00360037, 0x00360038, 0x00360039, 0x00360061, 0x00360062, 0x00360063, 0x00360064, 0x00360065, 0x00360066,
            0x00370030, 0x00370031, 0x00370032, 0x00370033, 0x00370034, 0x00370035, 0x00370036, 0x00370037, 0x00370038, 0x00370039, 0x00370061, 0x00370062, 0x00370063, 0x00370064, 0x00370065, 0x00370066,
            0x00380030, 0x00380031, 0x00380032, 0x00380033, 0x00380034, 0x00380035, 0x00380036, 0x00380037, 0x00380038, 0x00380039, 0x00380061, 0x00380062, 0x00380063, 0x00380064, 0x00380065, 0x00380066,
            0x00390030, 0x00390031, 0x00390032, 0x00390033, 0x00390034, 0x00390035, 0x00390036, 0x00390037, 0x00390038, 0x00390039, 0x00390061, 0x00390062, 0x00390063, 0x00390064, 0x00390065, 0x00390066,
            0x00610030, 0x00610031, 0x00610032, 0x00610033, 0x00610034, 0x00610035, 0x00610036, 0x00610037, 0x00610038, 0x00610039, 0x00610061, 0x00610062, 0x00610063, 0x00610064, 0x00610065, 0x00610066,
            0x00620030, 0x00620031, 0x00620032, 0x00620033, 0x00620034, 0x00620035, 0x00620036, 0x00620037, 0x00620038, 0x00620039, 0x00620061, 0x00620062, 0x00620063, 0x00620064, 0x00620065, 0x00620066,
            0x00630030, 0x00630031, 0x00630032, 0x00630033, 0x00630034, 0x00630035, 0x00630036, 0x00630037, 0x00630038, 0x00630039, 0x00630061, 0x00630062, 0x00630063, 0x00630064, 0x00630065, 0x00630066,
            0x00640030, 0x00640031, 0x00640032, 0x00640033, 0x00640034, 0x00640035, 0x00640036, 0x00640037, 0x00640038, 0x00640039, 0x00640061, 0x00640062, 0x00640063, 0x00640064, 0x00640065, 0x00640066,
            0x00650030, 0x00650031, 0x00650032, 0x00650033, 0x00650034, 0x00650035, 0x00650036, 0x00650037, 0x00650038, 0x00650039, 0x00650061, 0x00650062, 0x00650063, 0x00650064, 0x00650065, 0x00650066,
            0x00660030, 0x00660031, 0x00660032, 0x00660033, 0x00660034, 0x00660035, 0x00660036, 0x00660037, 0x00660038, 0x00660039, 0x00660061, 0x00660062, 0x00660063, 0x00660064, 0x00660065, 0x00660066
        };

        internal static readonly uint[] s_utf16LookupHexLowerLE =
        {
            0x00300030, 0x00310030, 0x00320030, 0x00330030, 0x00340030, 0x00350030, 0x00360030, 0x00370030, 0x00380030, 0x00390030, 0x00610030, 0x00620030, 0x00630030, 0x00640030, 0x00650030, 0x00660030,
            0x00300031, 0x00310031, 0x00320031, 0x00330031, 0x00340031, 0x00350031, 0x00360031, 0x00370031, 0x00380031, 0x00390031, 0x00610031, 0x00620031, 0x00630031, 0x00640031, 0x00650031, 0x00660031,
            0x00300032, 0x00310032, 0x00320032, 0x00330032, 0x00340032, 0x00350032, 0x00360032, 0x00370032, 0x00380032, 0x00390032, 0x00610032, 0x00620032, 0x00630032, 0x00640032, 0x00650032, 0x00660032,
            0x00300033, 0x00310033, 0x00320033, 0x00330033, 0x00340033, 0x00350033, 0x00360033, 0x00370033, 0x00380033, 0x00390033, 0x00610033, 0x00620033, 0x00630033, 0x00640033, 0x00650033, 0x00660033,
            0x00300034, 0x00310034, 0x00320034, 0x00330034, 0x00340034, 0x00350034, 0x00360034, 0x00370034, 0x00380034, 0x00390034, 0x00610034, 0x00620034, 0x00630034, 0x00640034, 0x00650034, 0x00660034,
            0x00300035, 0x00310035, 0x00320035, 0x00330035, 0x00340035, 0x00350035, 0x00360035, 0x00370035, 0x00380035, 0x00390035, 0x00610035, 0x00620035, 0x00630035, 0x00640035, 0x00650035, 0x00660035,
            0x00300036, 0x00310036, 0x00320036, 0x00330036, 0x00340036, 0x00350036, 0x00360036, 0x00370036, 0x00380036, 0x00390036, 0x00610036, 0x00620036, 0x00630036, 0x00640036, 0x00650036, 0x00660036,
            0x00300037, 0x00310037, 0x00320037, 0x00330037, 0x00340037, 0x00350037, 0x00360037, 0x00370037, 0x00380037, 0x00390037, 0x00610037, 0x00620037, 0x00630037, 0x00640037, 0x00650037, 0x00660037,
            0x00300038, 0x00310038, 0x00320038, 0x00330038, 0x00340038, 0x00350038, 0x00360038, 0x00370038, 0x00380038, 0x00390038, 0x00610038, 0x00620038, 0x00630038, 0x00640038, 0x00650038, 0x00660038,
            0x00300039, 0x00310039, 0x00320039, 0x00330039, 0x00340039, 0x00350039, 0x00360039, 0x00370039, 0x00380039, 0x00390039, 0x00610039, 0x00620039, 0x00630039, 0x00640039, 0x00650039, 0x00660039,
            0x00300061, 0x00310061, 0x00320061, 0x00330061, 0x00340061, 0x00350061, 0x00360061, 0x00370061, 0x00380061, 0x00390061, 0x00610061, 0x00620061, 0x00630061, 0x00640061, 0x00650061, 0x00660061,
            0x00300062, 0x00310062, 0x00320062, 0x00330062, 0x00340062, 0x00350062, 0x00360062, 0x00370062, 0x00380062, 0x00390062, 0x00610062, 0x00620062, 0x00630062, 0x00640062, 0x00650062, 0x00660062,
            0x00300063, 0x00310063, 0x00320063, 0x00330063, 0x00340063, 0x00350063, 0x00360063, 0x00370063, 0x00380063, 0x00390063, 0x00610063, 0x00620063, 0x00630063, 0x00640063, 0x00650063, 0x00660063,
            0x00300064, 0x00310064, 0x00320064, 0x00330064, 0x00340064, 0x00350064, 0x00360064, 0x00370064, 0x00380064, 0x00390064, 0x00610064, 0x00620064, 0x00630064, 0x00640064, 0x00650064, 0x00660064,
            0x00300065, 0x00310065, 0x00320065, 0x00330065, 0x00340065, 0x00350065, 0x00360065, 0x00370065, 0x00380065, 0x00390065, 0x00610065, 0x00620065, 0x00630065, 0x00640065, 0x00650065, 0x00660065,
            0x00300066, 0x00310066, 0x00320066, 0x00330066, 0x00340066, 0x00350066, 0x00360066, 0x00370066, 0x00380066, 0x00390066, 0x00610066, 0x00620066, 0x00630066, 0x00640066, 0x00650066, 0x00660066
        };
    }
}