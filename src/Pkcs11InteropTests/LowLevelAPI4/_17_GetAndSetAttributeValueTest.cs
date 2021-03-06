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
using Net.Pkcs11Interop.LowLevelAPI4;
using NUnit.Framework;

namespace Net.Pkcs11Interop.Tests.LowLevelAPI4
{
    /// <summary>
    /// C_GetAttributeValue and C_SetAttributeValue tests.
    /// </summary>
    [TestFixture()]
    public class _17_GetAndSetAttributeValueTest
    {
        /// <summary>
        /// C_GetAttributeValue test.
        /// </summary>
        [Test()]
        public void _01_GetAttributeValueTest()
        {
            if (UnmanagedLong.Size != 4)
                Assert.Inconclusive("Test cannot be executed on this platform");

            CKR rv = CKR.CKR_OK;
            
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath))
            {
                rv = pkcs11.C_Initialize(Settings.InitArgs4);
                if ((rv != CKR.CKR_OK) && (rv != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED))
                    Assert.Fail(rv.ToString());
                
                // Find first slot with token present
                uint slotId = Helpers.GetUsableSlot(pkcs11);
                
                uint session = CK.CK_INVALID_HANDLE;
                rv = pkcs11.C_OpenSession(slotId, (CKF.CKF_SERIAL_SESSION | CKF.CKF_RW_SESSION), IntPtr.Zero, IntPtr.Zero, ref session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                // Login as normal user
                rv = pkcs11.C_Login(session, CKU.CKU_USER, Settings.NormalUserPinArray, Convert.ToUInt32(Settings.NormalUserPinArray.Length));
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                // Create object
                uint objectId = CK.CK_INVALID_HANDLE;
                rv = Helpers.CreateDataObject(pkcs11, session, ref objectId);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Prepare list of empty attributes we want to read
                CK_ATTRIBUTE[] template = new CK_ATTRIBUTE[2];
                template[0] = CkaUtils.CreateAttribute(CKA.CKA_LABEL);
                template[1] = CkaUtils.CreateAttribute(CKA.CKA_VALUE);

                // Get size of each individual attribute value in first call
                rv = pkcs11.C_GetAttributeValue(session, objectId, template, Convert.ToUInt32(template.Length));
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // In LowLevelAPI we have to allocate unmanaged memory for attribute value
                for (int i = 0; i < template.Length; i++)
                    template[i].value = UnmanagedMemory.Allocate(Convert.ToInt32(template[i].valueLen));

                // Get attribute value in second call
                rv = pkcs11.C_GetAttributeValue(session, objectId, template, Convert.ToUInt32(template.Length));
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Do something interesting with attribute value
                byte[] ckaLabel = UnmanagedMemory.Read(template[0].value, Convert.ToInt32(template[0].valueLen));
                Assert.IsTrue(Convert.ToBase64String(ckaLabel) == Convert.ToBase64String(Settings.ApplicationNameArray));

                // In LowLevelAPI we have to free unmanaged memory taken by attributes
                for (int i = 0; i < template.Length; i++)
                {
                    UnmanagedMemory.Free(ref template[i].value);
                    template[i].valueLen = 0;
                }

                rv = pkcs11.C_DestroyObject(session, objectId);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                rv = pkcs11.C_Logout(session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                rv = pkcs11.C_CloseSession(session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                rv = pkcs11.C_Finalize(IntPtr.Zero);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
            }
        }

        /// <summary>
        /// C_SetAttributeValue test.
        /// </summary>
        [Test()]
        public void _02_SetAttributeValueTest()
        {
            if (UnmanagedLong.Size != 4)
                Assert.Inconclusive("Test cannot be executed on this platform");

            CKR rv = CKR.CKR_OK;
            
            using (Pkcs11 pkcs11 = new Pkcs11(Settings.Pkcs11LibraryPath))
            {
                rv = pkcs11.C_Initialize(Settings.InitArgs4);
                if ((rv != CKR.CKR_OK) && (rv != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED))
                    Assert.Fail(rv.ToString());
                
                // Find first slot with token present
                uint slotId = Helpers.GetUsableSlot(pkcs11);
                
                uint session = CK.CK_INVALID_HANDLE;
                rv = pkcs11.C_OpenSession(slotId, (CKF.CKF_SERIAL_SESSION | CKF.CKF_RW_SESSION), IntPtr.Zero, IntPtr.Zero, ref session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                // Login as normal user
                rv = pkcs11.C_Login(session, CKU.CKU_USER, Settings.NormalUserPinArray, Convert.ToUInt32(Settings.NormalUserPinArray.Length));
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                // Create object
                uint objectId = CK.CK_INVALID_HANDLE;
                rv = Helpers.CreateDataObject(pkcs11, session, ref objectId);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // Prepare list of attributes we want to set
                CK_ATTRIBUTE[] template = new CK_ATTRIBUTE[2];
                template[0] = CkaUtils.CreateAttribute(CKA.CKA_LABEL, "Hello world");
                template[1] = CkaUtils.CreateAttribute(CKA.CKA_VALUE, "New data object content");

                // Set attributes
                rv = pkcs11.C_SetAttributeValue(session, objectId, template, Convert.ToUInt32(template.Length));
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());

                // In LowLevelAPI we have to free unmanaged memory taken by attributes
                for (int i = 0; i < template.Length; i++)
                {
                    UnmanagedMemory.Free(ref template[i].value);
                    template[i].valueLen = 0;
                }
                
                rv = pkcs11.C_DestroyObject(session, objectId);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                rv = pkcs11.C_Logout(session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                rv = pkcs11.C_CloseSession(session);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
                
                rv = pkcs11.C_Finalize(IntPtr.Zero);
                if (rv != CKR.CKR_OK)
                    Assert.Fail(rv.ToString());
            }
        }
    }
}

