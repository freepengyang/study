using System.Collections;
using System.Collections.Generic;
using baozhu;
using TABLE;
using UnityEngine;

/// <summary>
///  宠物技能tips自适应类型
/// </summary>
public enum SkillTipsAdaptiveType
{
    BottomRight, //右下
    BottomLeft, //左下
    TopLeft, //左上
    TopRight, //右上
}

public partial class UIWarPetSkillTipsPanel : UIBasePanel
{
    public override void Init()
    {
        base.Init();
        AddCollider();
    }

    public override void Show()
    {
        base.Show();
    }

    public void OpenWarPetSkillTipsPanel(int id, SkillTipsAdaptiveType type = SkillTipsAdaptiveType.BottomRight)
    {
        Panel.alpha = 0;
        TABLE.SKILL skill;
        if (SkillTableManager.Instance.TryGetValue(id, out skill))
        {
            if (skill.type == 6 || skill.type == 7)
            {
                mlb_skill_name_beidong.text = skill.name;
                mlb_skill_name_beidong.gameObject.SetActive(true);
                mlb_skill_name.gameObject.SetActive(false);
                mlb_skill_lv.gameObject.SetActive(false);
            }
            else
            {
                mlb_skill_name.text = skill.name;
                mlb_skill_lv.text = CSString.Format(mlb_skill_lv.FormatStr, skill.level);
                mlb_skill_name_beidong.gameObject.SetActive(false);
                mlb_skill_name.gameObject.SetActive(true);
                mlb_skill_lv.gameObject.SetActive(true);
            }

            msp_skill_icon.spriteName = skill.icon;
            mlb_skill_desc.text = CSString.Format(mlb_skill_desc.FormatStr, skill.description);
            if (skill.cdTime > 0)
            {
                mlb_skill_cd_time.text = CSString.Format(mlb_skill_cd_time.FormatStr, (float) skill.cdTime / 1000);
                mlb_skill_cd_time.transform.localPosition =
                    mlb_skill_desc.transform.localPosition + Vector3.down * mlb_skill_desc.height;
                mlb_skill_cd_time.gameObject.SetActive(true);
            }
            else
            {
                mlb_skill_cd_time.height = 0;
                mlb_skill_cd_time.gameObject.SetActive(false);
            }

            //天赋解锁等级
            mlb_talentUnlock.gameObject.SetActive(false);
            var dicChongWuHeXin = ChongwuHexinTableManager.Instance.array.gItem.handles;
            var dicChongWuTianFu = ChongwuTianfuTableManager.Instance.array.gItem.handles;
            for (int k = 0, max = dicChongWuHeXin.Length; k < max; ++k)
            {
                var hexinItem = dicChongWuHeXin[k].Value as TABLE.CHONGWUHEXIN;
                if (skill.skillGroup == hexinItem.para1 ||
                    skill.skillGroup == hexinItem.para2 ||
                    skill.skillGroup == hexinItem.para3 ||
                    skill.skillGroup == hexinItem.para4)
                {
                    for (int j = 0, jmax = dicChongWuTianFu.Length; j < jmax; ++j)
                    {
                        var tianfuItem = dicChongWuTianFu[j].Value as TABLE.CHONGWUTIANFU;
                        if (tianfuItem.talentid == hexinItem.id)
                        {
                            mlb_talentUnlock.text = CSString.Format(1745, tianfuItem.unlockinlevel);
                            mlb_talentUnlock.gameObject.SetActive(true);
                        }
                    }
                }
            }

            msp_bg.height = 122 + mlb_skill_desc.height + mlb_skill_cd_time.height;
            if (mlb_talentUnlock.gameObject.activeSelf)
            {
                mlb_talentUnlock.transform.localPosition = !mlb_skill_cd_time.gameObject.activeSelf
                        ? mlb_skill_desc.transform.localPosition + Vector3.down * mlb_skill_desc.height
                        : mlb_skill_cd_time.transform.localPosition + Vector3.down * mlb_skill_cd_time.height;

                msp_bg.height += mlb_talentUnlock.height;
            }

            // //技能无冷却
            // if (skill.cdTime <= 0)
            //     mlb_skill_cd_time.text = CSString.Format(1723);

        //     if (go == null)
        //     {
        //         Vector2 localPoint = Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        //         UIPrefabTrans.localPosition = localPoint;
        //         switch (type)
        //         {
        //             case SkillTipsAdaptiveType.BottomRight:
        //                 UIPrefabTrans.localPosition += new Vector3(msp_bg.width / 2, -msp_bg.height / 2, 0);
        //                 break;
        //             case SkillTipsAdaptiveType.BottomLeft:
        //                 UIPrefabTrans.localPosition += new Vector3(-msp_bg.width / 2, -msp_bg.height / 2, 0);
        //                 break;
        //             case SkillTipsAdaptiveType.TopLeft:
        //                 UIPrefabTrans.localPosition += new Vector3(-msp_bg.width / 2, msp_bg.height / 2, 0);
        //                 break;
        //             case SkillTipsAdaptiveType.TopRight:
        //                 UIPrefabTrans.localPosition += new Vector3(msp_bg.width / 2, msp_bg.height / 2, 0);
        //                 break;
        //         }
        //     }
        //     else
        //     {
        //         Vector3 vec3 = go.transform.localPosition;
        //         UISprite sp = go.transform.GetComponent<UISprite>();
        //         float halfX = 0;
        //         float halfY = 0;
        //         if (sp != null)
        //         {
        //             halfX = sp.width / 2;
        //             halfY = sp.height / 2;
        //         }
        //
        //         switch (type)
        //         {
        //             case SkillTipsAdaptiveType.BottomRight:
        //                 UIPrefabTrans.localPosition = vec3 + new Vector3(halfX, -halfY, 0);
        //                 break;
        //             case SkillTipsAdaptiveType.BottomLeft:
        //                 UIPrefabTrans.localPosition = vec3 + new Vector3(-halfX, -halfY, 0);
        //                 break;
        //             case SkillTipsAdaptiveType.TopLeft:
        //                 UIPrefabTrans.localPosition = vec3 + new Vector3(-halfX, halfY, 0);
        //                 break;
        //             case SkillTipsAdaptiveType.TopRight:
        //                 UIPrefabTrans.localPosition = vec3 + new Vector3(halfX, halfY, 0);
        //                 break;
        //         }
        //     }
        }

        CSGame.Sington.StartCoroutine(SetPanelAlpha(1));
    }

    IEnumerator SetPanelAlpha(float value)
    {
        yield return null;
        Panel.alpha = 1f;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}