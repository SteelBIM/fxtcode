if(exists(select companyid From UserInfo with(nolock) where UserName=@username and UserPwd =@oldpwd and Valid = 1))
	begin --
		update UserInfo with(rowlock) set UserPwd=@userpwd where UserName=@username
		select 1
	end
else
	begin
		select 0
	end