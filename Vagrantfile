Vagrant.configure("2") do |config|
  config.vm.box = "ubuntu/jammy64"
  config.vm.network "forwarded_port", guest: 5001, host: 5011
  config.vm.network "private_network", ip: "192.168.33.10"
  config.vm.hostname = "addr-validator"
  
  config.vm.provision "shell", path: "scripts/bootstrap.sh"
end