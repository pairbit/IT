local val = redis.call('get', KEYS[1])
if (val == false) then
	return 0
else
	for _, eqVal in pairs(ARGV) do
		if val == eqVal then
			return 0
		end
	end
end
return redis.call('del', KEYS[1])