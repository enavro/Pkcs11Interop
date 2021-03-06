﻿/*
 *  Pkcs11Interop - Managed .NET wrapper for unmanaged PKCS#11 libraries
 *  Copyright (c) 2012-2015 JWC s.r.o. <http://www.jwc.sk>
 *  Author: Jaroslav Imrich <jimrich@jimrich.sk>
 *
 *  Licensing for open source projects:
 *  Pkcs11Interop is available under the terms of the GNU Affero General 
 *  Public License version 3 as published by the Free Software Foundation.
 *  Please see <http://www.gnu.org/licenses/agpl-3.0.html> for more details.
 *
 *  Licensing for other types of projects:
 *  Pkcs11Interop is available under the terms of flexible commercial license.
 *  Please contact JWC s.r.o. at <info@pkcs11interop.net> for more details.
 */

using System;
using System.Runtime.InteropServices;

namespace Net.Pkcs11Interop.LowLevelAPI4.MechanismParams
{
    /// <summary>
    /// Structure that provides the parameters to the CKM_CAMELLIA_CBC_ENCRYPT_DATA mechanism
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct CK_CAMELLIA_CBC_ENCRYPT_DATA_PARAMS
    {
        /// <summary>
        /// 16-octet initialization vector
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Iv;

        /// <summary>
        /// Pointer to data to encrypt
        /// </summary>
        public IntPtr Data;

        /// <summary>
        /// Length of data to encrypt
        /// </summary>
        public uint Length;
    }
}
