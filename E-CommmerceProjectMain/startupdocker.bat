docker pull roykumar/userimg
docker pull roykumar/productimg
docker pull roykumar/cartimg
docker pull roykumar/orderimg
docker pull roykumar/ocelotimg

docker network create --driver nat dockernetwork

docker run --network dockernetwork --name usercont -p 5293:80  -d userimg
docker run --network dockernetwork --name productcont -p 7186:80  -d productimg
docker run --network dockernetwork --name cartcont -p 7147:80  -d cartimg
docker run --network dockernetwork --name ordercont -p 7223:80  -d orderimg
docker run --network dockernetwork --name ocelotcont -p 7142:80  -d ocelotimg






