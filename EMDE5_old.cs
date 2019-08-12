using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD55
{
    static class EMDE5
    {
        private static uint rotate_left(uint value, int shift)
        {
            return value << shift | value >> (32 - shift);
        }

        private static uint F(uint X, uint Y, uint Z)
        {
            return ((X & Y) | ((~X) & Z));
        }

        private static uint G(uint X, uint Y, uint Z)
        {
            return (X & Z) | (Y & (~Z));
        }

        private static uint H(uint X, uint Y, uint Z)
        {
            return X ^ Y ^ Z;
        }

        private static uint I(uint X, uint Y, uint Z)
        {
            return Y ^ (X | (~Z));
        }

        public static string Create(string msg)
        {
            int length = msg.Length;
            int rest = length % 64;
            int size = 0;

            if (rest < 56)
                size = length - rest + 56 + 8;
            else
                size = length + 64 - rest + 56 + 8;
            char[] msg_for_decode = new char[size];
            for (int i = 0; i < length; i++)
                msg_for_decode[i] = msg[i];
            msg_for_decode[length] = (char)0x80;
            for(int i = length + 1; i<size; i++)
    	        msg_for_decode[i] = (char)0;
            
            Int64 bit_length = (uint)length * 8;
            msg_for_decode[size - 8] = (char)((bit_length >> 0) & 0xFF);
            msg_for_decode[size - 7] = (char)((bit_length >> 8) & 0xFF);
            msg_for_decode[size - 6] = (char)((bit_length >> 16) & 0xFF);
            msg_for_decode[size - 5] = (char)((bit_length >> 24) & 0xFF);
            msg_for_decode[size - 4] = (char)((bit_length >> 32) & 0xFF);
            msg_for_decode[size - 3] = (char)((bit_length >> 40) & 0xFF);
            msg_for_decode[size - 2] = (char)((bit_length >> 48) & 0xFF);
            msg_for_decode[size - 1] = (char)((bit_length >> 56) & 0xFF);
            //for (int i = 0; i < 8; i++)
            //    msg_for_decode[size - 8 + i] = (char)(bit_length >> (i << 3)); 
            uint A = 0x67452301, B = 0xEFCDAB89, C = 0x98BADCFE, D = 0x10325476;
            //uint[] X = new uint[msg_for_decode.Length];
            //for (int i = 0; i < X.Length; i++)
            //    X[i] = msg_for_decode[i];
            
            uint AA, BB, CC, DD;
            for(int i = 0; i < size / 64; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    uint[] X = new uint[16];
                    for (int j = 0; j < 16; j++)
                        X[j] = ((uint)msg_for_decode[64 * i + 16 * k + j]);
                    AA = A;
                    BB = B;
                    CC = C;
                    DD = D;

                    A = B + rotate_left((A + F(B, C, D) + X[0] + 0xD76AA478), 7);
                    D = A + rotate_left((D + F(A, B, C) + X[1] + 0xE8C7B756), 12);
                    C = D + rotate_left((C + F(D, A, B) + X[2] + 0x242070DB), 17);
                    B = C + rotate_left((B + F(C, D, A) + X[3] + 0xc1BDCEEE), 22);

                    A = B + rotate_left((A + F(B, C, D) + X[4] + 0xF57C0FAF), 7);
                    D = A + rotate_left((D + F(A, B, C) + X[5] + 0x4787C62A), 12);
                    C = D + rotate_left((C + F(D, A, B) + X[6] + 0xA8304613), 17);
                    B = C + rotate_left((B + F(C, D, A) + X[7] + 0xFD469501), 22);

                    A = B + rotate_left((A + F(B, C, D) + X[8] + 0x698098D8), 7);
                    D = A + rotate_left((D + F(A, B, C) + X[9] + 0x8B44f7AF), 12);
                    C = D + rotate_left((C + F(D, A, B) + X[10] + 0xFFFF5BB1), 17);
                    B = C + rotate_left((B + F(C, D, A) + X[11] + 0x895CD7BE), 22);

                    A = B + rotate_left((A + F(B, C, D) + X[12] + 0x6B901122), 7);
                    D = A + rotate_left((D + F(A, B, C) + X[13] + 0xFD987193), 12);
                    C = D + rotate_left((C + F(D, A, B) + X[14] + 0xA679438E), 17);
                    B = C + rotate_left((B + F(C, D, A) + X[15] + 0x49B40821), 22);


                    A = B + rotate_left((A + G(B, C, D) + X[1] + 0xF61E2562), 5);
                    D = A + rotate_left((D + G(A, B, C) + X[6] + 0xC040B340), 9);
                    C = D + rotate_left((C + G(D, A, B) + X[11] + 0x265E5A51), 14);
                    B = C + rotate_left((B + G(C, D, A) + X[0] + 0xE9B6C7AA), 20);

                    A = B + rotate_left((A + G(B, C, D) + X[5] + 0xD62F105D), 5);
                    D = A + rotate_left((D + G(A, B, C) + X[10] + 0x2441453), 9);
                    C = D + rotate_left((C + G(D, A, B) + X[15] + 0xD8A1E681), 14);
                    B = C + rotate_left((B + G(C, D, A) + X[4] + 0xE7D3FBC8), 20);

                    A = B + rotate_left((A + G(B, C, D) + X[9] + 0x21E1CDE6), 5);
                    D = A + rotate_left((D + G(A, B, C) + X[14] + 0xC33707D6), 9);
                    C = D + rotate_left((C + G(D, A, B) + X[3] + 0xF4D50D87), 14);
                    B = C + rotate_left((B + G(C, D, A) + X[8] + 0x455A14ED), 20);

                    A = B + rotate_left((A + G(B, C, D) + X[13] + 0xA9E3E905), 5);
                    D = A + rotate_left((D + G(A, B, C) + X[2] + 0xFCEFA3F8), 9);
                    C = D + rotate_left((C + G(D, A, B) + X[7] + 0x676F02D9), 14);
                    B = C + rotate_left((B + G(C, D, A) + X[12] + 0x8D2A4C8A), 20);


                    A = B + rotate_left((A + H(B, C, D) + X[5] + 0xFFFA3942), 4);
                    D = A + rotate_left((D + H(A, B, C) + X[8] + 0x8771F681), 11);
                    C = D + rotate_left((C + H(D, A, B) + X[11] + 0x6D9D6122), 16);
                    B = C + rotate_left((B + H(C, D, A) + X[14] + 0xFDE5380C), 23);

                    A = B + rotate_left((A + H(B, C, D) + X[1] + 0xA4BEEA44), 4);
                    D = A + rotate_left((D + H(A, B, C) + X[4] + 0x4BDECFA9), 11);
                    C = D + rotate_left((C + H(D, A, B) + X[7] + 0xF6BB4B60), 16);
                    B = C + rotate_left((B + H(C, D, A) + X[10] + 0xBEBFBC70), 23);

                    A = B + rotate_left((A + H(B, C, D) + X[13] + 0x289B7EC6), 4);
                    D = A + rotate_left((D + H(A, B, C) + X[0] + 0xEAA127FA), 11);
                    C = D + rotate_left((C + H(D, A, B) + X[3] + 0xD4EF3085), 16);
                    B = C + rotate_left((B + H(C, D, A) + X[6] + 0x4881D05), 23);

                    A = B + rotate_left((A + H(B, C, D) + X[9] + 0xD9D4D039), 4);
                    D = A + rotate_left((D + H(A, B, C) + X[12] + 0xE6DB99E5), 11);
                    C = D + rotate_left((C + H(D, A, B) + X[15] + 0x1FA27CF8), 16);
                    B = C + rotate_left((B + H(C, D, A) + X[2] + 0xC4AC5665), 23);


                    A = B + rotate_left((A + I(B, C, D) + X[0] + 0xF4292244), 6);
                    D = A + rotate_left((D + I(A, B, C) + X[7] + 0x432AFF97), 10);
                    C = D + rotate_left((C + I(D, A, B) + X[14] + 0xAB9423A7), 15);
                    B = C + rotate_left((B + I(C, D, A) + X[5] + 0xFC93A039), 21);

                    A = B + rotate_left((A + I(B, C, D) + X[12] + 0x655B59C3), 6);
                    D = A + rotate_left((D + I(A, B, C) + X[3] + 0x8F0CCC92), 10);
                    C = D + rotate_left((C + I(D, A, B) + X[10] + 0xFFEFF47D), 15);
                    B = C + rotate_left((B + I(C, D, A) + X[1] + 0x85845DD1), 21);

                    A = B + rotate_left((A + I(B, C, D) + X[8] + 0x6FA87E4F), 6);
                    D = A + rotate_left((D + I(A, B, C) + X[15] + 0xFE2CE6E0), 10);
                    C = D + rotate_left((C + I(D, A, B) + X[6] + 0xA3014314), 15);
                    B = C + rotate_left((B + I(C, D, A) + X[13] + 0x4E0811A1), 21);

                    A = B + rotate_left((A + I(B, C, D) + X[4] + 0xF7537E82), 6);
                    D = A + rotate_left((D + I(A, B, C) + X[11] + 0xBD3AF235), 10);
                    C = D + rotate_left((C + I(D, A, B) + X[2] + 0x2AD7D2BB), 15);
                    B = C + rotate_left((B + I(C, D, A) + X[9] + 0xED86D391), 21);

                    A += AA;
                    B += BB;
                    C += CC;
                    D += DD;
                }
            }
            string result = "";
            char[] res = new char[64];
            uint[] src = { A, B, C, D};
            for (int i = 0; i < 4; i++)
            {
                result += Convert.ToByte(src[i] & 0xff).ToString("X");
                result += Convert.ToByte((src[i] >> 8) & 0xff).ToString("X");
                result += Convert.ToByte((src[i] >> 16) & 0xff).ToString("X");
                result += Convert.ToByte((src[i] >> 24) & 0xff).ToString("X");
            }
            return result;
        }

    }
}
