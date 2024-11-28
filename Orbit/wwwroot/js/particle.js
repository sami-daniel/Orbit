// Initialize particlesJS with configuration for particle animation
particlesJS("particles-js", {
  particles: {
    number: {
      value: 80, // Number of particles to display
      density: {
        enable: true, // Enable particle density
        value_area: 800, // Area within which particles are distributed
      },
    },
    color: {
      value: "#ffffff", // Color of the particles
    },
    shape: {
      type: "circle", // Shape of the particles (e.g., circle, edge, triangle)
      stroke: {
        width: 0, // Width of the particle border
        color: "#000000", // Border color (unused when width is 0)
      },
      polygon: {
        nb_sides: 5, // Number of sides for polygonal shapes
      },
      image: {
        src: "img/github.svg", // Path to an image for custom shapes
        width: 100, // Width of the image
        height: 98, // Height of the image
      },
    },
    opacity: {
      value: 0.5, // Default opacity of the particles
      random: false, // Whether opacity is randomized
      anim: {
        enable: false, // Enable animation for opacity changes
        speed: 1, // Speed of opacity animation
        opacity_min: 0.1, // Minimum opacity value
        sync: false, // Sync animation across particles
      },
    },
    size: {
      value: 3, // Default size of the particles
      random: true, // Whether size is randomized
      anim: {
        enable: false, // Enable animation for size changes
        speed: 40, // Speed of size animation
        size_min: 0.1, // Minimum size value
        sync: false, // Sync animation across particles
      },
    },
    line_linked: {
      enable: true, // Enable lines connecting particles
      distance: 150, // Maximum distance for particles to link
      color: "#ffffff", // Color of the connecting lines
      opacity: 0.4, // Opacity of the connecting lines
      width: 1, // Width of the connecting lines
    },
    move: {
      enable: true, // Enable particle movement
      speed: 6, // Speed of particle movement
      direction: "none", // Direction of particle movement
      random: false, // Whether movement is randomized
      straight: false, // Whether particles move in a straight line
      out_mode: "out", // Behavior when particles move out of bounds ("out", "bounce")
      bounce: false, // Whether particles bounce when hitting boundaries
      attract: {
        enable: false, // Enable attraction between particles
        rotateX: 600, // Attraction effect on the X-axis
        rotateY: 1200, // Attraction effect on the Y-axis
      },
    },
  },
  interactivity: {
    detect_on: "canvas", // Area to detect user interaction (canvas or window)
    events: {
      onhover: {
        enable: true, // Enable particle interaction on hover
        mode: "repulse", // Interaction mode on hover (e.g., grab, bubble, repulse)
      },
      onclick: {
        enable: true, // Enable particle interaction on click
        mode: "push", // Interaction mode on click (e.g., push, remove)
      },
      resize: true, // Adjust particles when the canvas is resized
    },
    modes: {
      grab: {
        distance: 400, // Distance for grab interaction
        line_linked: {
          opacity: 1, // Opacity of the linked line during grab interaction
        },
      },
      bubble: {
        distance: 400, // Distance for bubble interaction
        size: 40, // Size of particles during bubble interaction
        duration: 2, // Duration of bubble effect
        opacity: 8, // Opacity during bubble interaction
        speed: 3, // Speed of bubble interaction effect
      },
      repulse: {
        distance: 200, // Distance for repulse interaction
        duration: 0.4, // Duration of the repulse effect
      },
      push: {
        particles_nb: 4, // Number of particles added on push interaction
      },
      remove: {
        particles_nb: 2, // Number of particles removed on remove interaction
      },
    },
  },
  retina_detect: true, // Enable retina display support for higher quality rendering
});
