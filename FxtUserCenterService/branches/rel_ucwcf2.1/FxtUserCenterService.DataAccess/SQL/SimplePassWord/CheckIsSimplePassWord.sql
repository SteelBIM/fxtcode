if(exists(select id From SimplePassWord with(nolock) where simplepassword =@simplepassword))
	begin --
		select 1
	end
else
	begin
		select 0
	end