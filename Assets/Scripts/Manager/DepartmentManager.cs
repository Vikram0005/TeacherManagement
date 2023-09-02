using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepartmentManager : MonoBehaviour
{
    void Start()
    {
        //DepartmentRequest departmentRequest = new DepartmentRequest();
        //departmentRequest.name = "BCA";
        //departmentRequest.shortName="BCA";
        //departmentRequest.status = true;
        //departmentRequest.code = "BCA101";


        //APIManager.Instance.CallAPI<DepartmentRequest, Response<EmptyResponse>>(PacketType.AddDepartment, departmentRequest,OnResponse);

        APIManager.Instance.CallAPI<Response<DepartmentListRes>>(PacketType.GetAllDepartment, OnResponseList);
    }

    private void OnResponseList(Response<DepartmentListRes> obj)
    {
        print(obj.data.departmentRes.Count);
    }

    private void OnResponse(Response<EmptyResponse> response)
    {
        print(response.message);
    }

}
