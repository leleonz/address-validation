# Set defaults (Update as required)
DEFAULT_DOTNET_SDK_VERSION="7.0"

### Changes after this line is most probably not necessary ###

# Set arguments
DOTNET_SDK_VERSION=${1:-$DEFAULT_DOTNET_SDK_VERSION}

# Download and Install Packages
apt-get update
apt-get install -y dotnet-sdk-${DOTNET_SDK_VERSION}