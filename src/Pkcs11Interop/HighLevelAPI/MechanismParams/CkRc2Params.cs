/*
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
using Net.Pkcs11Interop.Common;

namespace Net.Pkcs11Interop.HighLevelAPI.MechanismParams
{
    /// <summary>
    /// Parameters for the CKM_RC2_ECB and CKM_RC2_MAC mechanisms
    /// </summary>
    public class CkRc2Params : IMechanismParams
    {
        /// <summary>
        /// Platform specific CkRc2Params
        /// </summary>
        private HighLevelAPI4.MechanismParams.CkRc2Params _params4 = null;

        /// <summary>
        /// Platform specific CkRc2Params
        /// </summary>
        private HighLevelAPI8.MechanismParams.CkRc2Params _params8 = null;
        
        /// <summary>
        /// Initializes a new instance of the CkRc2Params class.
        /// </summary>
        /// <param name='effectiveBits'>Effective number of bits in the RC2 search space</param>
        public CkRc2Params(ulong effectiveBits)
        {
            if (UnmanagedLong.Size == 4)
                _params4 = new HighLevelAPI4.MechanismParams.CkRc2Params(Convert.ToUInt32(effectiveBits));
            else
                _params8 = new HighLevelAPI8.MechanismParams.CkRc2Params(effectiveBits);
        }
        
        #region IMechanismParams

        /// <summary>
        /// Returns managed object that can be marshaled to an unmanaged block of memory
        /// </summary>
        /// <returns>A managed object holding the data to be marshaled. This object must be an instance of a formatted class.</returns>
        public object ToMarshalableStructure()
        {
            if (UnmanagedLong.Size == 4)
                return _params4.ToMarshalableStructure();
            else
                return _params8.ToMarshalableStructure();
        }
        
        #endregion
    }
}
