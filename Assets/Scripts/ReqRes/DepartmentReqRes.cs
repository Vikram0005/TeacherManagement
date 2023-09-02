using System.Collections.Generic;

[System.Serializable]
public class DepartmentRequest
{
    public string name;
    public string shortName;
    public bool status;
    public string code;
}

[System.Serializable]
public class DepartmentRes
{
    public long id;
    public string name;
    public string shortName;
}

[System.Serializable]
public class DepartmentListRes
{
    public List<DepartmentRes> departmentRes = new List<DepartmentRes>();
}

