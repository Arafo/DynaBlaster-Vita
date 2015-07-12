using System;
using System.Collections;
using System.Collections.Generic;

using Sce.PlayStation.Core.Imaging;

namespace DynaBlasterVita
{
	public class Animation {
	
		public enum Direction {UP, DOWN, LEFT, RIGHT};
	
	    private int frameCount;                
	    private int frameDelay;                 
	    private int currentFrame;              
	    private Direction animationDirection;        
	     
	    private int totalFrames;               
	    private bool stopped;
	    private List<Frame> frames = new List<Frame>();
	
	    public Animation(Sprite[] frames, int frameDelay) {
	        this.frameDelay = frameDelay;
	        this.stopped = true;
	
	        for (int i = 0; i < frames.Length; i++) {
	            addFrame(frames[i], frameDelay);
	        }
	
	        this.frameCount = 0;
	        this.frameDelay = frameDelay;
	        this.currentFrame = 0;
	        this.animationDirection = Direction.DOWN;
	        this.totalFrames = this.frames.Count;
	        }
	        
	    public Animation(Sprite[] frames, int frameDelay, Direction dir) {
	        this.frameDelay = frameDelay;
	        this.stopped = true;
	
	        for (int i = 0; i < frames.Length; i++) {
	            addFrame(frames[i], frameDelay);
	        }
	
	        this.frameCount = 0;
	        this.frameDelay = frameDelay;
	        this.currentFrame = 0;
	        this.animationDirection = dir;
	        this.totalFrames = this.frames.Count;
	
	    }
	
	    public void start() {
	        if (!stopped) {
	            return;
	        }
	
	        if (frames.Count == 0) {
	            return;
	        }
	        
	        stopped = false;
	    }
	
	    public void stop() {
	        if (frames.Count == 0) {
	            return;
	        }
	
	        stopped = true;
	    }
	
	    public void restart() {
	        if (frames.Count == 0) {
	            return;
	        }
	
	        stopped = false;
	        currentFrame = 0;
	    }
	
	    public void reset() {
	        this.stopped = true;
	        this.frameCount = 0;
	        this.currentFrame = 0;
	    }
	    
	    public bool finalFrame() {
	        return currentFrame == totalFrames - 1 && frameCount == frameDelay;
	    }
	
	    private void addFrame(Sprite frame, int duration) {
	        if (duration <= 0) {
	            //throw new RuntimeException("Invalid duration: " + duration);
	        }
	        frames.Add(new Frame(frame, duration));
	        currentFrame = 0;
	    }
	
	    public Sprite getSprite() {
			return frames[currentFrame].getImg();
	        //return frames.get(currentFrame).getImg();
	    }
	    
	    public Direction getAnimationDirection() {
			return animationDirection;
		}
	    
	    public void setAnimationDirection(Direction d) {
	    	animationDirection = d;
		}
	
	    public void tick() {
	        if (!stopped) {
	            frameCount++;
	
	            if (frameCount > frameDelay) {
	                frameCount = 0;
	                currentFrame += 1;
	
	                if (currentFrame > totalFrames - 1) {
	                    currentFrame = 0;
	                }
	                else if (currentFrame < 0) {
	                    currentFrame = totalFrames - 1;
	                }
	            }
	        }
	
	    }
	
		public int getCurrentFrame() {
			return currentFrame;
		}
	}
}
	
	
