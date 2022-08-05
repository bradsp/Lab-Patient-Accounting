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
                ListViewItem lvi = new ListViewItem(rule.ToString());
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
                    TreeNode groupNode = new TreeNode(group.ToString())
                    {
                        Tag = group
                    };

                    var details = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Detail" && c.GroupId == group.GroupId);

                    foreach(var detail in details)
                    {
                        TreeNode detailNode = new TreeNode(detail.ToString())
                        {
                            Tag = detail
                        };

                        groupNode.Nodes.Add(detailNode);
                    }

                    var subGroups = rule.claimValidationRuleCriteria.Where(c => c.LineType == "SubGroup" && c.ParentGroupId == group.GroupId);

                    foreach (var subGroup in subGroups)
                    {
                        TreeNode subgroupNode = new TreeNode(subGroup.ToString())
                        {
                            Tag = subGroup
                        };

                        var sgDetails = rule.claimValidationRuleCriteria.Where(c => c.LineType == "Detail" && c.GroupId == subGroup.GroupId);
                        if (sgDetails != null)
                        {
                            foreach (var detail in sgDetails)
                            {
                                TreeNode detailNode = new TreeNode(detail.ToString())
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
            var rule = (ClaimValidationRule)tvRuleHierarchy.Nodes[0].Tag;

            tbErrorText.Text = rule.ErrorText;
            tbRuleName.Text = rule.RuleName;
            tbRuleDescription.Text = rule.Description;
            effectiveDate.Value = rule.EffectiveDate;
            endEffectiveDate.Value = rule.EndEffectiveDate;

            if (tvRuleHierarchy.SelectedNode.Level > 0)
            {
                var detail = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;
                cbMemberName.SelectedItem = detail.MemberName;
                cbLineType.SelectedItem = detail.LineType;
                cbOperator.SelectedItem = detail.Operator;
                tbTargetValue.Text = detail.TargetValue;

                Type type = Type.GetType($"LabBilling.Core.Models.{detail.Class},LabBilling Core");
                if(type != null)
                    propertyList = ObjectProperties.GetProperties(type).ToList();

                cbMemberName.Items.Clear();
                cbMemberName.Items.AddRange(propertyList.ToArray());
                cbMemberName.SelectedItem = detail.MemberName;
            }

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

        private void removeDetailButton_Click(object sender, EventArgs e)
        {

        }

        private void addDetailButton_Click(object sender, EventArgs e)
        {
            var selectedNode = tvRuleHierarchy.SelectedNode;

            ClaimValidationRuleCriterion newCriterion = new ClaimValidationRuleCriterion();

            TreeNode newNode = new TreeNode();

            if (tvRuleHierarchy.SelectedNode.Level == 0) //this is the root node
            {
                ClaimValidationRule rule = (ClaimValidationRule)tvRuleHierarchy.SelectedNode.Tag;
                newCriterion.RuleId = rule.RuleId;
                newCriterion.Class = "Account";

                newNode.Tag = newCriterion;
                selectedNode.Nodes.Add(newNode);
            }
            else
            {
                //get parent node
                var selectedTag = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;

                newCriterion.RuleId = selectedTag.RuleId;
                newCriterion.ParentGroupId = selectedTag.ParentGroupId;
                newCriterion.GroupId = selectedTag.GroupId;
                newCriterion.Class = selectedTag.Class;

                newNode.Tag = newCriterion;

                if (selectedTag.LineType == "Group" || selectedTag.LineType == "SubGroup") // add a detail node
                {
                    selectedNode.Nodes.Add(newNode);
                }
                else if (selectedTag.LineType == "Detail") // add a detail node to parent node
                {
                    var parentNode = tvRuleHierarchy.SelectedNode.Parent;
                    parentNode.Nodes.Add(newNode);
                }
            }

            cbLineType.SelectedIndex = -1;
            cbMemberName.SelectedIndex = -1;
            cbOperator.SelectedIndex = -1;
            tbTargetValue.Text = String.Empty;

            tvRuleHierarchy.SelectedNode = newNode;
            newNode.EnsureVisible();

        }

        private void saveCriteraButton_Click(object sender, EventArgs e)
        {
            ClaimValidationRuleCriterion detail = new ClaimValidationRuleCriterion();
            var selectedNode = tvRuleHierarchy.SelectedNode;
            var selectedTag = (ClaimValidationRuleCriterion)tvRuleHierarchy.SelectedNode.Tag;

            selectedTag.TargetValue = tbTargetValue.Text;
            selectedTag.LineType = cbLineType.SelectedItem.ToString();
            selectedTag.MemberName = cbMemberName.SelectedItem.ToString();
            selectedTag.Operator = cbOperator.SelectedItem.ToString();

            selectedNode.Tag = selectedTag;
            selectedNode.Text = selectedTag.ToString();

        }
    }
}
