local val = redis.call('hget', KEYS[1], ARGV[1])
redis.call('hset', KEYS[1], ARGV[1], ARGV[2])
return val