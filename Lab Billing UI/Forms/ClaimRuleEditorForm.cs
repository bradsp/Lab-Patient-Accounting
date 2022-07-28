using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MetroFramework.Forms;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using RFClassLibrary;

namespace LabBilling.Forms
{
    public partial class ClaimRuleEditorForm : MetroForm
    {
        public ClaimRuleEditorForm()
        {
            InitializeComponent();
        }

        private List<ClaimValidationRule> claimRules;
        private List<string> propertyList;

        private void ClaimRuleEditorForm_Load(object sender, EventArgs e)
        {
            ClaimValidationRuleRepository claimRuleRepository = new ClaimValidationRuleRepository(Helper.ConnVal);

            claimRules = claimRuleRepository.GetRules();

            listRules.View = View.Details;

            listRules.FullRowSelect = true;

            listRules.Columns.Add("RuleName", 200, HorizontalAlignment.Left);
            listRules.Columns.Add("RuleId", 10, HorizontalAlignment.Right);

            foreach (ClaimValidationRule rule in claimRules)
            {
                ListViewItem lvi = new ListViewItem(rule.RuleName);
                lvi.SubItems.Add(rule.RuleId.ToString());

                listRules.Items.Add(lvi);
            }
        }

        private void listRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load the tree view control with the rule
            if(listRules.SelectedItems.Count > 0)
            {
                int selectedRule = listRules.SelectedItems[0].Index;

                ClaimValidationRule rule = claimRules[selectedRule];

                TreeNode parentNode = new TreeNode(rule.RuleName.ToString())
                {
                    Tag = rule
                };

                var groups = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Group");

                foreach (var group in groups)
                {
                    TreeNode groupNode = new TreeNode($"{group.GroupId} {group.MemberName} {group.Operator} {group.TargetValue}")
                    {
                        Tag = group
                    };

                    var details = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Detail" && c.GroupId == group.GroupId);

                    foreach(var detail in details)
                    {
                        TreeNode detailNode = new TreeNode($"{detail.MemberName} {detail.Operator} {detail.TargetValue}")
                        {
                            Tag = detail
                        };

                        groupNode.Nodes.Add(detailNode);
                    }

                    var subGroups = rule.claimValidationRuleCriteria.Where(c => c.LineType == "SubGroup" && c.ParentGroupId == group.GroupId);

                    foreach (var subGroup in subGroups)
                    {
                        TreeNode subgroupNode = new TreeNode($"{subGroup.MemberName} {subGroup.Operator} {subGroup.TargetValue}")
                        {
                            Tag = subGroup
                        };

                        var sgDetails = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Detail" && c.GroupId == subGroup.GroupId);
                        if (sgDetails != null)
                        {
                            foreach (var detail in sgDetails)
                            {
                                TreeNode detailNode = new TreeNode($"{detail.MemberName} {detail.Operator} {detail.TargetValue}")
                                {
                                    Tag = detail
                                };

                                subgroupNode.Nodes.Add(detailNode);
                            }
                        }
                        groupNode.Nodes.Add(subgroupNode);

                    }

                    parentNode.Nodes.Add(groupNode);
                }
                tvRuleHierarchy.Nodes.Add(parentNode);
                tvRuleHierarchy.ExpandAll();
            }
        }

        private void tvRuleHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var detail = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;
            var rule = (ClaimValidationRule)tvRuleHierarchy.Nodes[0].Tag;

            tbErrorText.Text = rule.ErrorText;
            tbRuleName.Text = rule.RuleName;
            tbRuleDescription.Text = rule.Description;
            tbMemberName.Text = detail.MemberName;
            cbMemberName.SelectedItem = detail.MemberName;
            effectiveDate.Value = rule.EffectiveDate;
            endEffectiveDate.Value = rule.EndEffectiveDate;
            cbLineType.SelectedItem = detail.LineType;
            cbOperator.SelectedItem = detail.Operator;
            tbTargetValue.Text = detail.TargetValue;

            Type type = Type.GetType($"LabBilling.Core.Models.{detail.Class},LabBilling Core");

            propertyList = ObjectProperties.GetProperties(type).ToList();

            cbMemberName.Items.Clear();
            cbMemberName.Items.AddRange(propertyList.ToArray());
            cbMemberName.SelectedItem = detail.MemberName;

        }

        private void toolStripAddCriteria_Click(object sender, EventArgs e)
        {

        }

        private void toolStripAddGroup_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDeleteCriteria_Click(object sender, EventArgs e)
        {

        }

        private void addRuleButton_Click(object sender, EventArgs e)
        {

        }

        private void saveRuleButton_Click(object sender, EventArgs e)
        {

        }
    }
}
