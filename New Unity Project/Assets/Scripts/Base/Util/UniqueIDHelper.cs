using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//获取唯一id帮助类
public class UniqueIDHelper
{
    private int m_dwMin;
    private int m_dwMax;
    private int m_dwUniqueID;
    private HashSet<int> m_UniqueIDSet = new HashSet<int>();

    private bool m_bFindFailed;
    private bool m_bCheckUsed;
    public UniqueIDHelper(int min = 0, int max = 0x7FFFFFFF, bool check_used = true)
    {
        m_dwMin = min;
        m_dwMax = max;
        m_dwUniqueID = min;
        m_UniqueIDSet.Clear();
        m_bFindFailed = false;
        m_bCheckUsed = check_used;
    }
    public int GetUnique()
    {
        int return_id = m_dwUniqueID;
        if (m_dwUniqueID == m_dwMax)
        {
            if (m_bCheckUsed && m_bFindFailed)
            {
                m_dwUniqueID = m_dwMin;
                Debugger.Log("uniqueid越界");
                return m_dwMin;
            }
            m_dwUniqueID = m_dwMin;
        }
        else
        {
            m_dwUniqueID++;
        }
        if (m_bCheckUsed == false)
        {
            return return_id;
        }
        if (IsExsitUnique(return_id))
        {
            m_bFindFailed = true;
            return GetUnique();
        }
        m_bFindFailed = false;
        m_UniqueIDSet.Add(return_id);
        return return_id;
    }

    public bool IsExsitUnique(int id)
    {
        if (m_UniqueIDSet.Contains(id))
        {
            return true;
        }
        return false;
    }
    public bool AddUnique(int id)
    {
        if (IsExsitUnique(id))
        {
            Debugger.Log("加入新的点失败");
            return false;
        }
        m_UniqueIDSet.Add(id);
        return true;
    }
    public void RemoveUniqueID(int id)
    {
        if (m_UniqueIDSet.Contains(id))
        {
            m_UniqueIDSet.Remove(id);
        }
    }
    public void RemoveAllUniqueID()
    {
        m_UniqueIDSet.Clear();
    }
}