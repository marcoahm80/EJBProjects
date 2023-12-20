Select Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,isnull(ReferenceNotes,'') ReferenceNotes,LaborQty,ActiveLabor,Procesed, EM.Site from [dbo].[ProdReport] PR
inner join [dbo].[UserMES] EM on PR.EmployeeNum = EM.EmployeeID
where Procesed = 0 and ActiveLabor = 0


Select Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,ScrapDate,ResourceGroup,ResourceId,isnull(ReferenceNotes,'') ReferenceNotes,ScrapQty,ReasonCode,WhseCode,BinNum,Procesed, EM.Site from [dbo].[ScrapReport] SR
inner join [dbo].[UserMES] EM on SR.EmployeeNum = EM.EmployeeID
where Procesed = 0 

Select Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,DownTimeStartDate,DownTimeEndDate,ResourceGroup,ResourceId,isnull(ReferenceNotes,'') ReferenceNotes,ReasonCode,Procesed,ActiveDowntime, EM.Site from [dbo].[DowntimeReport] DR
inner join [dbo].[UserMES] EM on DR.EmployeeNum = EM.EmployeeID
where Procesed = 0 and ActiveDowntime = 0


exec sp_help '[dbo].[DowntimeReport]'


--Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,StartDate,EndDate,ResourceGroup,ResourceId,ReferenceNotes,LaborQty,ActiveLabor,Procesed

--Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,ScrapDate,ResourceGroup,ResourceId,ReferenceNotes,ScrapQty,ReasonCode,WhseCode,BinNum,Procesed

--Id,EmployeeNum,JobNum,AssemblyNum,OpSeq,DownTimeStartDate,DownTimeEndDate,ResourceGroup,ResourceId,ReferenceNotes,ReasonCode,Procesed,ActiveDowntime