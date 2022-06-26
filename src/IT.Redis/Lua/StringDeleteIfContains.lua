local val = redis.call('get', KEYS[1])
if val ~= false then
	for _, eqVal in pairs(ARGV) do
		if val == eqVal then
			return redis.call('del', KEYS[1])
		end
	end
end
return 0