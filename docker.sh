### HOW TO USE ###
# 1) Make sure Docker is running
# 2) "cd MazeGenerator"
# 3) "git tag" (determine next highest tag)
# 4) Call this script with next highest tag as arg "./docker.sh v10"
# 5) If no new tag is needed, call with "latest" ex: "./docker.sh latest"

VERSION=$1

echo "*** DOCKER BUILD BEGIN ***"

echo "*** TAGGING & VERSIONING ***"

if [ $VERSION != "latest" ]; then
	git tag $VERSION
	git push --tags
else
	VERSION=$(git for-each-ref refs/tags --sort=-taggerdate --format='%(refname:short)' --count=1)
fi

git for-each-ref refs/tags --sort=-taggerdate --format='%(refname:short)' --count=1 > wwwroot/version.txt

echo "*** DOCKER BUILD ***"

docker build -t digitalwizardry/mazegenerator .