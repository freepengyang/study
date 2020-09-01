using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class YvVoiceMgr_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::YvVoiceMgr);
            args = new Type[]{};
            method = type.GetMethod("get_Instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Instance_0);
            args = new Type[]{};
            method = type.GetMethod("get_mLoginType", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_mLoginType_1);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("Init", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Init_2);
            args = new Type[]{typeof(System.Action), typeof(System.Boolean)};
            method = type.GetMethod("Logout", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Logout_3);
            args = new Type[]{typeof(System.Int32), typeof(System.String), typeof(System.String), typeof(System.Int32), typeof(System.String), typeof(System.Action)};
            method = type.GetMethod("Login", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Login_4);
            args = new Type[]{typeof(System.String), typeof(System.Action)};
            method = type.GetMethod("PlayAudio", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayAudio_5);
            args = new Type[]{};
            method = type.GetMethod("StopRecord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopRecord_6);
            args = new Type[]{typeof(System.Action<System.String>), typeof(System.Action<System.String>), typeof(System.Action<System.Int32, System.String, System.String, System.String, System.Int32>)};
            method = type.GetMethod("StartRecord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StartRecord_7);
            args = new Type[]{};
            method = type.GetMethod("get_isOpenVoiceSpeak", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isOpenVoiceSpeak_8);
            args = new Type[]{};
            method = type.GetMethod("get_isOpenVoiceLister", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_isOpenVoiceLister_9);
            args = new Type[]{};
            method = type.GetMethod("StopVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopVoice_10);
            args = new Type[]{};
            method = type.GetMethod("OpenVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OpenVoice_11);
            args = new Type[]{};
            method = type.GetMethod("EnableSpeaker", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, EnableSpeaker_12);
            args = new Type[]{};
            method = type.GetMethod("StopAudio", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopAudio_13);

            field = type.GetField("SpeakingMemberChangedCB", flag);
            app.RegisterCLRFieldGetter(field, get_SpeakingMemberChangedCB_0);
            app.RegisterCLRFieldSetter(field, set_SpeakingMemberChangedCB_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_SpeakingMemberChangedCB_0, AssignFromStack_SpeakingMemberChangedCB_0);
            field = type.GetField("VoiceSpeakChangeCallBack", flag);
            app.RegisterCLRFieldGetter(field, get_VoiceSpeakChangeCallBack_1);
            app.RegisterCLRFieldSetter(field, set_VoiceSpeakChangeCallBack_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_VoiceSpeakChangeCallBack_1, AssignFromStack_VoiceSpeakChangeCallBack_1);
            field = type.GetField("LoginRoomCallBack", flag);
            app.RegisterCLRFieldGetter(field, get_LoginRoomCallBack_2);
            app.RegisterCLRFieldSetter(field, set_LoginRoomCallBack_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_LoginRoomCallBack_2, AssignFromStack_LoginRoomCallBack_2);
            field = type.GetField("LogoutRoomCallBack", flag);
            app.RegisterCLRFieldGetter(field, get_LogoutRoomCallBack_3);
            app.RegisterCLRFieldSetter(field, set_LogoutRoomCallBack_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_LogoutRoomCallBack_3, AssignFromStack_LogoutRoomCallBack_3);
            field = type.GetField("ShowTipsCallBack", flag);
            app.RegisterCLRFieldGetter(field, get_ShowTipsCallBack_4);
            app.RegisterCLRFieldSetter(field, set_ShowTipsCallBack_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_ShowTipsCallBack_4, AssignFromStack_ShowTipsCallBack_4);
            field = type.GetField("LoginTypeChangeCallBack", flag);
            app.RegisterCLRFieldGetter(field, get_LoginTypeChangeCallBack_5);
            app.RegisterCLRFieldSetter(field, set_LoginTypeChangeCallBack_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_LoginTypeChangeCallBack_5, AssignFromStack_LoginTypeChangeCallBack_5);
            field = type.GetField("isRuningRecord", flag);
            app.RegisterCLRFieldGetter(field, get_isRuningRecord_6);
            app.RegisterCLRFieldSetter(field, set_isRuningRecord_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_isRuningRecord_6, AssignFromStack_isRuningRecord_6);
            field = type.GetField("isLogin", flag);
            app.RegisterCLRFieldGetter(field, get_isLogin_7);
            app.RegisterCLRFieldSetter(field, set_isLogin_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_isLogin_7, AssignFromStack_isLogin_7);
            field = type.GetField("isCancelLuying", flag);
            app.RegisterCLRFieldGetter(field, get_isCancelLuying_8);
            app.RegisterCLRFieldSetter(field, set_isCancelLuying_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_isCancelLuying_8, AssignFromStack_isCancelLuying_8);
            field = type.GetField("mSpeakingMembers", flag);
            app.RegisterCLRFieldGetter(field, get_mSpeakingMembers_9);
            app.RegisterCLRFieldSetter(field, set_mSpeakingMembers_9);
            app.RegisterCLRFieldBinding(field, CopyToStack_mSpeakingMembers_9, AssignFromStack_mSpeakingMembers_9);
            field = type.GetField("AutoPlayAudioList", flag);
            app.RegisterCLRFieldGetter(field, get_AutoPlayAudioList_10);
            app.RegisterCLRFieldSetter(field, set_AutoPlayAudioList_10);
            app.RegisterCLRFieldBinding(field, CopyToStack_AutoPlayAudioList_10, AssignFromStack_AutoPlayAudioList_10);


        }


        static StackObject* get_Instance_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::YvVoiceMgr.Instance;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_mLoginType_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.mLoginType;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* Init_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @voiceOpenId = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Init(@voiceOpenId);

            return __ret;
        }

        static StackObject* Logout_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isLogoutGame = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action @response = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Logout(@response, @isLogoutGame);

            return __ret;
        }

        static StackObject* Login_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 7);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @response = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @roomID = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @level = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @uid = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            System.String @name = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 6);
            System.Int32 @loginType = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 7);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Login(@loginType, @name, @uid, @level, @roomID, @response);

            return __ret;
        }

        static StackObject* PlayAudio_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @response = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @url = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.PlayAudio(@url, @response);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* StopRecord_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopRecord();

            return __ret;
        }

        static StackObject* StartRecord_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action<System.Int32, System.String, System.String, System.String, System.Int32> @action = (System.Action<System.Int32, System.String, System.String, System.String, System.Int32>)typeof(System.Action<System.Int32, System.String, System.String, System.String, System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action<System.String> @UploadResponse = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Action<System.String> @luYingResponse = (System.Action<System.String>)typeof(System.Action<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StartRecord(@luYingResponse, @UploadResponse, @action);

            return __ret;
        }

        static StackObject* get_isOpenVoiceSpeak_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isOpenVoiceSpeak;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_isOpenVoiceLister_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.isOpenVoiceLister;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* StopVoice_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopVoice();

            return __ret;
        }

        static StackObject* OpenVoice_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.OpenVoice();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* EnableSpeaker_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.EnableSpeaker();

            return __ret;
        }

        static StackObject* StopAudio_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::YvVoiceMgr instance_of_this_method = (global::YvVoiceMgr)typeof(global::YvVoiceMgr).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopAudio();

            return __ret;
        }


        static object get_SpeakingMemberChangedCB_0(ref object o)
        {
            return ((global::YvVoiceMgr)o).SpeakingMemberChangedCB;
        }

        static StackObject* CopyToStack_SpeakingMemberChangedCB_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).SpeakingMemberChangedCB;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_SpeakingMemberChangedCB_0(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).SpeakingMemberChangedCB = (System.Action)v;
        }

        static StackObject* AssignFromStack_SpeakingMemberChangedCB_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @SpeakingMemberChangedCB = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).SpeakingMemberChangedCB = @SpeakingMemberChangedCB;
            return ptr_of_this_method;
        }

        static object get_VoiceSpeakChangeCallBack_1(ref object o)
        {
            return ((global::YvVoiceMgr)o).VoiceSpeakChangeCallBack;
        }

        static StackObject* CopyToStack_VoiceSpeakChangeCallBack_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).VoiceSpeakChangeCallBack;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_VoiceSpeakChangeCallBack_1(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).VoiceSpeakChangeCallBack = (System.Action<System.Boolean>)v;
        }

        static StackObject* AssignFromStack_VoiceSpeakChangeCallBack_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Boolean> @VoiceSpeakChangeCallBack = (System.Action<System.Boolean>)typeof(System.Action<System.Boolean>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).VoiceSpeakChangeCallBack = @VoiceSpeakChangeCallBack;
            return ptr_of_this_method;
        }

        static object get_LoginRoomCallBack_2(ref object o)
        {
            return ((global::YvVoiceMgr)o).LoginRoomCallBack;
        }

        static StackObject* CopyToStack_LoginRoomCallBack_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).LoginRoomCallBack;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_LoginRoomCallBack_2(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).LoginRoomCallBack = (System.Action<System.Int32>)v;
        }

        static StackObject* AssignFromStack_LoginRoomCallBack_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32> @LoginRoomCallBack = (System.Action<System.Int32>)typeof(System.Action<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).LoginRoomCallBack = @LoginRoomCallBack;
            return ptr_of_this_method;
        }

        static object get_LogoutRoomCallBack_3(ref object o)
        {
            return ((global::YvVoiceMgr)o).LogoutRoomCallBack;
        }

        static StackObject* CopyToStack_LogoutRoomCallBack_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).LogoutRoomCallBack;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_LogoutRoomCallBack_3(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).LogoutRoomCallBack = (System.Action<System.Int32>)v;
        }

        static StackObject* AssignFromStack_LogoutRoomCallBack_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32> @LogoutRoomCallBack = (System.Action<System.Int32>)typeof(System.Action<System.Int32>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).LogoutRoomCallBack = @LogoutRoomCallBack;
            return ptr_of_this_method;
        }

        static object get_ShowTipsCallBack_4(ref object o)
        {
            return ((global::YvVoiceMgr)o).ShowTipsCallBack;
        }

        static StackObject* CopyToStack_ShowTipsCallBack_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).ShowTipsCallBack;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ShowTipsCallBack_4(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).ShowTipsCallBack = (System.Action<System.Int32, System.String>)v;
        }

        static StackObject* AssignFromStack_ShowTipsCallBack_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action<System.Int32, System.String> @ShowTipsCallBack = (System.Action<System.Int32, System.String>)typeof(System.Action<System.Int32, System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).ShowTipsCallBack = @ShowTipsCallBack;
            return ptr_of_this_method;
        }

        static object get_LoginTypeChangeCallBack_5(ref object o)
        {
            return ((global::YvVoiceMgr)o).LoginTypeChangeCallBack;
        }

        static StackObject* CopyToStack_LoginTypeChangeCallBack_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).LoginTypeChangeCallBack;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_LoginTypeChangeCallBack_5(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).LoginTypeChangeCallBack = (System.Action)v;
        }

        static StackObject* AssignFromStack_LoginTypeChangeCallBack_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @LoginTypeChangeCallBack = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).LoginTypeChangeCallBack = @LoginTypeChangeCallBack;
            return ptr_of_this_method;
        }

        static object get_isRuningRecord_6(ref object o)
        {
            return global::YvVoiceMgr.isRuningRecord;
        }

        static StackObject* CopyToStack_isRuningRecord_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::YvVoiceMgr.isRuningRecord;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isRuningRecord_6(ref object o, object v)
        {
            global::YvVoiceMgr.isRuningRecord = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isRuningRecord_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isRuningRecord = ptr_of_this_method->Value == 1;
            global::YvVoiceMgr.isRuningRecord = @isRuningRecord;
            return ptr_of_this_method;
        }

        static object get_isLogin_7(ref object o)
        {
            return ((global::YvVoiceMgr)o).isLogin;
        }

        static StackObject* CopyToStack_isLogin_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).isLogin;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isLogin_7(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).isLogin = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isLogin_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isLogin = ptr_of_this_method->Value == 1;
            ((global::YvVoiceMgr)o).isLogin = @isLogin;
            return ptr_of_this_method;
        }

        static object get_isCancelLuying_8(ref object o)
        {
            return ((global::YvVoiceMgr)o).isCancelLuying;
        }

        static StackObject* CopyToStack_isCancelLuying_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).isCancelLuying;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_isCancelLuying_8(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).isCancelLuying = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_isCancelLuying_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @isCancelLuying = ptr_of_this_method->Value == 1;
            ((global::YvVoiceMgr)o).isCancelLuying = @isCancelLuying;
            return ptr_of_this_method;
        }

        static object get_mSpeakingMembers_9(ref object o)
        {
            return ((global::YvVoiceMgr)o).mSpeakingMembers;
        }

        static StackObject* CopyToStack_mSpeakingMembers_9(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::YvVoiceMgr)o).mSpeakingMembers;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mSpeakingMembers_9(ref object o, object v)
        {
            ((global::YvVoiceMgr)o).mSpeakingMembers = (System.Collections.Generic.List<System.Int64>)v;
        }

        static StackObject* AssignFromStack_mSpeakingMembers_9(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.Int64> @mSpeakingMembers = (System.Collections.Generic.List<System.Int64>)typeof(System.Collections.Generic.List<System.Int64>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::YvVoiceMgr)o).mSpeakingMembers = @mSpeakingMembers;
            return ptr_of_this_method;
        }

        static object get_AutoPlayAudioList_10(ref object o)
        {
            return global::YvVoiceMgr.AutoPlayAudioList;
        }

        static StackObject* CopyToStack_AutoPlayAudioList_10(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = global::YvVoiceMgr.AutoPlayAudioList;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_AutoPlayAudioList_10(ref object o, object v)
        {
            global::YvVoiceMgr.AutoPlayAudioList = (System.Collections.Generic.List<System.String>)v;
        }

        static StackObject* AssignFromStack_AutoPlayAudioList_10(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<System.String> @AutoPlayAudioList = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            global::YvVoiceMgr.AutoPlayAudioList = @AutoPlayAudioList;
            return ptr_of_this_method;
        }



    }
}
