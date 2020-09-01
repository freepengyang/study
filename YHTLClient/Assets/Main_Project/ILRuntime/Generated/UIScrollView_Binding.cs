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
    unsafe class UIScrollView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIScrollView);
            args = new Type[]{};
            method = type.GetMethod("get_panel", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_panel_0);
            args = new Type[]{};
            method = type.GetMethod("get_currentMomentum", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_currentMomentum_1);
            args = new Type[]{};
            method = type.GetMethod("ResetPosition", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResetPosition_2);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("ScrollImmidate", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ScrollImmidate_3);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("SetDragAmount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDragAmount_4);
            args = new Type[]{};
            method = type.GetMethod("get_shouldMoveVertically", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_shouldMoveVertically_5);
            args = new Type[]{typeof(System.Single), typeof(System.Single), typeof(System.Boolean)};
            method = type.GetMethod("SetDragAmount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDragAmount_6);
            args = new Type[]{};
            method = type.GetMethod("UpdateScrollbars", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateScrollbars_7);
            args = new Type[]{};
            method = type.GetMethod("InvalidateBounds", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InvalidateBounds_8);
            args = new Type[]{};
            method = type.GetMethod("get_canMoveVertically", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_canMoveVertically_9);
            args = new Type[]{};
            method = type.GetMethod("get_canMoveHorizontally", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_canMoveHorizontally_10);

            field = type.GetField("centerOnChild", flag);
            app.RegisterCLRFieldGetter(field, get_centerOnChild_0);
            app.RegisterCLRFieldSetter(field, set_centerOnChild_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_centerOnChild_0, AssignFromStack_centerOnChild_0);
            field = type.GetField("onDragFinished", flag);
            app.RegisterCLRFieldGetter(field, get_onDragFinished_1);
            app.RegisterCLRFieldSetter(field, set_onDragFinished_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_onDragFinished_1, AssignFromStack_onDragFinished_1);
            field = type.GetField("horizontalScrollBar", flag);
            app.RegisterCLRFieldGetter(field, get_horizontalScrollBar_2);
            app.RegisterCLRFieldSetter(field, set_horizontalScrollBar_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_horizontalScrollBar_2, AssignFromStack_horizontalScrollBar_2);
            field = type.GetField("verticalScrollBar", flag);
            app.RegisterCLRFieldGetter(field, get_verticalScrollBar_3);
            app.RegisterCLRFieldSetter(field, set_verticalScrollBar_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_verticalScrollBar_3, AssignFromStack_verticalScrollBar_3);
            field = type.GetField("momentumAmount", flag);
            app.RegisterCLRFieldGetter(field, get_momentumAmount_4);
            app.RegisterCLRFieldSetter(field, set_momentumAmount_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_momentumAmount_4, AssignFromStack_momentumAmount_4);
            field = type.GetField("movement", flag);
            app.RegisterCLRFieldGetter(field, get_movement_5);
            app.RegisterCLRFieldSetter(field, set_movement_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_movement_5, AssignFromStack_movement_5);
            field = type.GetField("disableDragIfFits", flag);
            app.RegisterCLRFieldGetter(field, get_disableDragIfFits_6);
            app.RegisterCLRFieldSetter(field, set_disableDragIfFits_6);
            app.RegisterCLRFieldBinding(field, CopyToStack_disableDragIfFits_6, AssignFromStack_disableDragIfFits_6);
            field = type.GetField("onStoppedMoving", flag);
            app.RegisterCLRFieldGetter(field, get_onStoppedMoving_7);
            app.RegisterCLRFieldSetter(field, set_onStoppedMoving_7);
            app.RegisterCLRFieldBinding(field, CopyToStack_onStoppedMoving_7, AssignFromStack_onStoppedMoving_7);
            field = type.GetField("restrictWithinPanel", flag);
            app.RegisterCLRFieldGetter(field, get_restrictWithinPanel_8);
            app.RegisterCLRFieldSetter(field, set_restrictWithinPanel_8);
            app.RegisterCLRFieldBinding(field, CopyToStack_restrictWithinPanel_8, AssignFromStack_restrictWithinPanel_8);


        }


        static StackObject* get_panel_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.panel;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_currentMomentum_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.currentMomentum;

            if (ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder != null) {
                ILRuntime.Runtime.Generated.CLRBindings.s_UnityEngine_Vector3_Binding_Binder.PushValue(ref result_of_this_method, __intp, __ret, __mStack);
                return __ret + 1;
            } else {
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
            }
        }

        static StackObject* ResetPosition_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ResetPosition();

            return __ret;
        }

        static StackObject* ScrollImmidate_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.ScrollImmidate(@value);

            return __ret;
        }

        static StackObject* SetDragAmount_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @strength = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetDragAmount(@x, @y, @strength);

            return __ret;
        }

        static StackObject* get_shouldMoveVertically_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.shouldMoveVertically;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* SetDragAmount_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @updateScrollbars = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @y = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Single @x = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetDragAmount(@x, @y, @updateScrollbars);

            return __ret;
        }

        static StackObject* UpdateScrollbars_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateScrollbars();

            return __ret;
        }

        static StackObject* InvalidateBounds_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InvalidateBounds();

            return __ret;
        }

        static StackObject* get_canMoveVertically_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.canMoveVertically;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* get_canMoveHorizontally_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::UIScrollView instance_of_this_method = (global::UIScrollView)typeof(global::UIScrollView).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.canMoveHorizontally;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }


        static object get_centerOnChild_0(ref object o)
        {
            return ((global::UIScrollView)o).centerOnChild;
        }

        static StackObject* CopyToStack_centerOnChild_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).centerOnChild;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_centerOnChild_0(ref object o, object v)
        {
            ((global::UIScrollView)o).centerOnChild = (global::UICenterOnChild)v;
        }

        static StackObject* AssignFromStack_centerOnChild_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UICenterOnChild @centerOnChild = (global::UICenterOnChild)typeof(global::UICenterOnChild).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIScrollView)o).centerOnChild = @centerOnChild;
            return ptr_of_this_method;
        }

        static object get_onDragFinished_1(ref object o)
        {
            return ((global::UIScrollView)o).onDragFinished;
        }

        static StackObject* CopyToStack_onDragFinished_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).onDragFinished;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onDragFinished_1(ref object o, object v)
        {
            ((global::UIScrollView)o).onDragFinished = (System.Action)v;
        }

        static StackObject* AssignFromStack_onDragFinished_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onDragFinished = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIScrollView)o).onDragFinished = @onDragFinished;
            return ptr_of_this_method;
        }

        static object get_horizontalScrollBar_2(ref object o)
        {
            return ((global::UIScrollView)o).horizontalScrollBar;
        }

        static StackObject* CopyToStack_horizontalScrollBar_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).horizontalScrollBar;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_horizontalScrollBar_2(ref object o, object v)
        {
            ((global::UIScrollView)o).horizontalScrollBar = (global::UIProgressBar)v;
        }

        static StackObject* AssignFromStack_horizontalScrollBar_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIProgressBar @horizontalScrollBar = (global::UIProgressBar)typeof(global::UIProgressBar).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIScrollView)o).horizontalScrollBar = @horizontalScrollBar;
            return ptr_of_this_method;
        }

        static object get_verticalScrollBar_3(ref object o)
        {
            return ((global::UIScrollView)o).verticalScrollBar;
        }

        static StackObject* CopyToStack_verticalScrollBar_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).verticalScrollBar;
            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_verticalScrollBar_3(ref object o, object v)
        {
            ((global::UIScrollView)o).verticalScrollBar = (global::UIProgressBar)v;
        }

        static StackObject* AssignFromStack_verticalScrollBar_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIProgressBar @verticalScrollBar = (global::UIProgressBar)typeof(global::UIProgressBar).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIScrollView)o).verticalScrollBar = @verticalScrollBar;
            return ptr_of_this_method;
        }

        static object get_momentumAmount_4(ref object o)
        {
            return ((global::UIScrollView)o).momentumAmount;
        }

        static StackObject* CopyToStack_momentumAmount_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).momentumAmount;
            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_momentumAmount_4(ref object o, object v)
        {
            ((global::UIScrollView)o).momentumAmount = (System.Single)v;
        }

        static StackObject* AssignFromStack_momentumAmount_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Single @momentumAmount = *(float*)&ptr_of_this_method->Value;
            ((global::UIScrollView)o).momentumAmount = @momentumAmount;
            return ptr_of_this_method;
        }

        static object get_movement_5(ref object o)
        {
            return ((global::UIScrollView)o).movement;
        }

        static StackObject* CopyToStack_movement_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).movement;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_movement_5(ref object o, object v)
        {
            ((global::UIScrollView)o).movement = (global::UIScrollView.Movement)v;
        }

        static StackObject* AssignFromStack_movement_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::UIScrollView.Movement @movement = (global::UIScrollView.Movement)typeof(global::UIScrollView.Movement).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIScrollView)o).movement = @movement;
            return ptr_of_this_method;
        }

        static object get_disableDragIfFits_6(ref object o)
        {
            return ((global::UIScrollView)o).disableDragIfFits;
        }

        static StackObject* CopyToStack_disableDragIfFits_6(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).disableDragIfFits;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_disableDragIfFits_6(ref object o, object v)
        {
            ((global::UIScrollView)o).disableDragIfFits = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_disableDragIfFits_6(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @disableDragIfFits = ptr_of_this_method->Value == 1;
            ((global::UIScrollView)o).disableDragIfFits = @disableDragIfFits;
            return ptr_of_this_method;
        }

        static object get_onStoppedMoving_7(ref object o)
        {
            return ((global::UIScrollView)o).onStoppedMoving;
        }

        static StackObject* CopyToStack_onStoppedMoving_7(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).onStoppedMoving;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_onStoppedMoving_7(ref object o, object v)
        {
            ((global::UIScrollView)o).onStoppedMoving = (System.Action)v;
        }

        static StackObject* AssignFromStack_onStoppedMoving_7(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Action @onStoppedMoving = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            ((global::UIScrollView)o).onStoppedMoving = @onStoppedMoving;
            return ptr_of_this_method;
        }

        static object get_restrictWithinPanel_8(ref object o)
        {
            return ((global::UIScrollView)o).restrictWithinPanel;
        }

        static StackObject* CopyToStack_restrictWithinPanel_8(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::UIScrollView)o).restrictWithinPanel;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static void set_restrictWithinPanel_8(ref object o, object v)
        {
            ((global::UIScrollView)o).restrictWithinPanel = (System.Boolean)v;
        }

        static StackObject* AssignFromStack_restrictWithinPanel_8(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Boolean @restrictWithinPanel = ptr_of_this_method->Value == 1;
            ((global::UIScrollView)o).restrictWithinPanel = @restrictWithinPanel;
            return ptr_of_this_method;
        }



    }
}
