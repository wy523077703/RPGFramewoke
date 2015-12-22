﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

public class AnimatorEditor : Editor {

    private static string defaultStateName = "idle";
    private static Dictionary<int, AnimatorState> combos;

    [MenuItem("Tools/Animator/Create AnimatorController")]
    public static void CreateAnimatorController() {
        AnimatorController controller = null;
        combos = new Dictionary<int, AnimatorState>();
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
            string path = AssetDatabase.GetAssetPath(obj);
            string[] str=path.Split('/');
            controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Animator/" + str[str.Length-1]+".controller");
        }
        if (controller == null)
            return;
       foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
       {
           string path = AssetDatabase.GetAssetPath(obj);
           if (!path.Contains(".FBX") || !path.Contains("@"))
               continue;
           AddParameter(path, controller);
       }
       AnimatorStateMachine rootMachine = controller.layers[0].stateMachine;
       AnimatorState defaultState = rootMachine.defaultState;
       ChildAnimatorState[] states = rootMachine.states;
       foreach (ChildAnimatorState state in states)
       {
           if (!rootMachine.defaultState.name.Equals(state.state.name))
           {
               //idle到其他state的condition
               AnimatorStateTransition transitionFrom = defaultState.AddTransition(state.state);
               transitionFrom.AddCondition(AnimatorConditionMode.If, 0f, state.state.name);
               transitionFrom.duration = 0;
               transitionFrom.exitTime = 0;
               //其他state到idle的condition
               AnimatorStateTransition transitionTo = state.state.AddTransition(defaultState);
               transitionTo.AddCondition(AnimatorConditionMode.IfNot, 0f, state.state.name);
               transitionTo.duration = 0;
               transitionTo.exitTime = 0;

               //设置连招
               if ("atk1".Equals(state.state.name))
                   combos.Add(1,state.state);
               else if ("atk2".Equals(state.state.name))
                   combos.Add(2, state.state);
               else if ("atk3".Equals(state.state.name))
                   combos.Add(3, state.state);
               else if ("atk4".Equals(state.state.name))
                   combos.Add(4, state.state);
           }
           BindBehaviour(state.state);
       }
       ConnectCombo(combos);
    }

    //绑定behaviour
    private static void BindBehaviour(AnimatorState state) { 
        switch(state.name){
            case "idle":
                state.AddStateMachineBehaviour<IdleBehaviour>();
                break;
            case "run":
                state.AddStateMachineBehaviour<RunBehaviour>();
                break;
            case "roll":
                state.AddStateMachineBehaviour<RollBehaviour>();
                break;
            case "dead":
                state.AddStateMachineBehaviour<DeadBehaviour>();
                break;
            case "atk1":
                state.AddStateMachineBehaviour<Atk1Behaviour>();
                break;
            case "atk2":
                state.AddStateMachineBehaviour<Atk2Behaviour>();
                break;
            case "atk3":
                state.AddStateMachineBehaviour<Atk3Behaviour>();
                break;
            case "atk4":
                state.AddStateMachineBehaviour<Atk4Behaviour>();
                break;
            case "skill1":
                state.AddStateMachineBehaviour<Skill1Behaviour>();
                break;
            case "skill2":
                state.AddStateMachineBehaviour<Skill2Behaviour>();
                break;
            case "hit":
                state.AddStateMachineBehaviour<HitBehaviour>();
                break;
        }
    }

    //给controller增加参数，参数名为motion名，且为bool类型
    private static void AddParameter(string path, AnimatorController controller)
    {
        AnimatorStateMachine sm = controller.layers[0].stateMachine;
        //根据动画文件读取它的Motion对象
        Motion motion = AssetDatabase.LoadAssetAtPath(path, typeof(Motion)) as Motion;
        string fileName = FetchFileName(path);       
        AnimatorState state = sm.AddState(fileName);
        //取出动画名子 添加到state里面
        state.motion = motion;
        //根据文件名增加参数
        if (!defaultStateName.Equals(fileName))
            controller.AddParameter(fileName, AnimatorControllerParameterType.Bool);
        else
            sm.defaultState = state;//设置默认state       
    }

    //根据动画文件路径获取文件名
    private static string FetchFileName(string path) {
        return path.Split('@')[1].Split('.')[0];
    }

    //设置连招的连接
    private static void ConnectCombo(Dictionary<int,AnimatorState> combos) {
        int len = combos.Count;
        for (int i = 1; i < len+1;i++ )
        {
            if (i != len) {
                AnimatorStateTransition transitionFrom = combos[i].AddTransition(combos[i+1]);
                transitionFrom.AddCondition(AnimatorConditionMode.If, 0f, combos[i+1].name);
                transitionFrom.duration = 0;
                transitionFrom.exitTime = 0;
            }
            
        }
    }

}
