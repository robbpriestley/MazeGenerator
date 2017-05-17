### HOW TO USE ###
# 1) "cd MazeGenerator"
# 2) "git tag" (determine next highest tag)
# 3) Call this script with next highest tag as arg "./docker.sh v1.1.003"
# 4) If no new tag is needed, call with "latest" ex: "./docker.sh latest"

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